using System.Collections.Generic;
using UnityEngine;

namespace ShootemUp
{
    public class ScrollingBackground : MonoBehaviour
    {
        public static ScrollingBackground Instance;

        [SerializeField]
        int _activeBackgroundsLimit = 3;

        [Space]
        [SerializeField]
        BackgroundLayer[] _backgroundLayers;

        public BackgroundLayer[] BackgroundLayers => _backgroundLayers;

        public int Layer => projectLayers.Backgrounds;

        Dictionary<string, GoCount> _backgroundGoCountByName = new Dictionary<string, GoCount>();
        Dictionary<string, BackgroundLayer> _backgroundLayerByName = new Dictionary<string, BackgroundLayer>();
        protected Dictionary<string, Queue<GameObject>> GoQueueByName = new Dictionary<string, Queue<GameObject>>();

        Transform _previewBackground;

        public Transform BackgroundsOnTreadmillHeirarchy { get; private set; }

        public float MainBackgroundPrefabWidth { get; private set; }
        public float MainBackgroundPrefabHeight { get; private set; }
        public Vector2 MainBackgroundPrefabDimensoins { get; private set; }

        private void Awake()
        {
            Instance = this;
            DestroyPreviewBackgrounds();
            InitBackgroundDimensons();
            CreateHierarchy();
            IndexLayersAndBackground();
            ActivateInitialBackgrounds();
        }

        private void OnValidate()
        {
            Instance = this;

            if (!CanProcessFirstBackground()) return;

            VerifyActiveBackgroundsLimit();
            VerifyCountValueNotZero();
            InitBackgroundDimensons();
            VerifyBackgroundCountValues();

            if (_backgroundLayers[0].AfterSequenceEnds == SequenceEndOptions.LoopFinallmage)
            {
                VerifyLastBGCount();
            }
        }

        private void VerifyActiveBackgroundsLimit()
        {
            if (_activeBackgroundsLimit < ProjectConstants.MinActiveBackgroundsLimit)
                _activeBackgroundsLimit = ProjectConstants.MinActiveBackgroundsLimit;
        }

        protected void VerifyCountValueNotZero()
        {
            for (int i = 0; i < _backgroundLayers[0].Backgrounds.Length; i++)
            {
                if (_backgroundLayers[0].Backgrounds[i]._count < 1)
                    _backgroundLayers[0].Backgrounds[i]._count = 1;
            }
        }

        public void InitBackgroundDimensons()
        {
            if (!CanProcessFirstBackground()) return;

            MainBackgroundPrefabWidth = Dimensions.FindWidth(_backgroundLayers[0].Backgrounds[0]._prefab);
            MainBackgroundPrefabHeight = Dimensions.FindHeight(_backgroundLayers[0].Backgrounds[0]._prefab);

            MainBackgroundPrefabDimensoins = new Vector2(MainBackgroundPrefabWidth, MainBackgroundPrefabHeight);
        }

        private void VerifyBackgroundCountValues()
        {
            foreach (BackgroundLayer bl in _backgroundLayers)
            {
                int totalBackgroundCount = 0;
                for (int i = 0; i < bl.Backgrounds.Length; i++)
                    totalBackgroundCount += bl.Backgrounds[i]._count;

                if (totalBackgroundCount < _activeBackgroundsLimit)
                {
                    int missingBackgrounds = _activeBackgroundsLimit - totalBackgroundCount;
                    bl.Backgrounds[bl.Backgrounds.Length - 1]._count += missingBackgrounds;
                }
            }
        }

        private void VerifyLastBGCount()
        {
            int lastBackgroundIndex = _backgroundLayers[0].Backgrounds.Length - 1;

            if (_backgroundLayers[0].Backgrounds[lastBackgroundIndex]._count < _activeBackgroundsLimit)
                _backgroundLayers[0].Backgrounds[lastBackgroundIndex]._count = _activeBackgroundsLimit;
        }

        protected void CreateHierarchy()
        {
            BackgroundsOnTreadmillHeirarchy = new GameObject("----- Background On Treadmill -----").transform;
        }

        private void IndexLayersAndBackground()
        {
            for (int i = 0; i < _backgroundLayers.Length; i++)
            {
                string layerName = StringTools.CombineNameAndNumber(ProjectConstants.LayerPrefix, i);
                PrepareAndIndexLayers(i, layerName);
                IndexBackgrounds(i, layerName);
            }
        }

        private void PrepareAndIndexLayers(int layerIndex, string layerName)
        {
            _backgroundLayers[layerIndex].TreadmillTransfrom = CreateTransformOnTreadmill(layerIndex, layerName);
            _backgroundLayers[layerIndex].BackgroundsSequence = CreateBackgroundsSequence(layerIndex, layerName);
            _backgroundLayers[layerIndex].StopAtFinalImageCounter = _activeBackgroundsLimit;
            _backgroundLayers[layerIndex].CanCountinueSpawning = true;

            _backgroundLayerByName.Add(layerName, _backgroundLayers[layerIndex]);
        }

        private Transform CreateTransformOnTreadmill(int layerIndex, string layerName)
        {
            Transform layerOnTreadmillHierarchy = new GameObject(layerName).transform;
            layerOnTreadmillHierarchy.position = Vector3.back * (_backgroundLayers[layerIndex].DepthIndex * Level.SpaceBetweenIndices);
            layerOnTreadmillHierarchy.parent = BackgroundsOnTreadmillHeirarchy;
            return layerOnTreadmillHierarchy;
        }

        private List<string> CreateBackgroundsSequence(int LayerIndex, string layerName)
        {
            GoCount[] backgrounds = _backgroundLayers[LayerIndex].Backgrounds;
            List<string> backgroundsSequence = new List<string>();

            for (int i = 0; i < backgrounds.Length; i++)
            {
                if (backgrounds[i]._prefab == null)
                {
                    Debug.Log("Background entry number: " + i + " in layer number: " + LayerIndex + " is missing its prefab.");
                    continue;
                }

                for (int j = 0; j < backgrounds[i]._count; j++)
                {
                    string backgroundName = layerName + backgrounds[i]._prefab.name;
                    backgroundsSequence.Add(backgroundName);
                }
            }
            return backgroundsSequence;
        }

        private void IndexBackgrounds(int layerIndex, string layerName)
        {
            for (int i = 0; i < _backgroundLayers[layerIndex].Backgrounds.Length; i++)
            {
                string newBackgroundName = layerName + _backgroundLayers[layerIndex].Backgrounds[i]._prefab.name;
                try
                {
                    _backgroundGoCountByName.Add(newBackgroundName, _backgroundLayers[layerIndex].Backgrounds[i]);
                }
                catch (System.ArgumentException)
                {
                    _backgroundGoCountByName[newBackgroundName]._count += _backgroundLayers[layerIndex].Backgrounds[i]._count;
                }
            }
        }

        private void ActivateInitialBackgrounds()
        {
            foreach (var kvp in _backgroundGoCountByName)
            {
                int maxCount = kvp.Value._count > _activeBackgroundsLimit ? _activeBackgroundsLimit : kvp.Value._count;
                string layerName = kvp.Key.Remove(ProjectConstants.LayerPrefixLength);

                if (!_backgroundLayerByName.ContainsKey(layerName))
                {
                    Debug.Log($"ScrollingBackground.cs: unable to find layer: {layerName}");
                    return;
                }

                BackgroundLayer backgroundLayer = _backgroundLayerByName[layerName];
                Queue<GameObject> GameObjectPool = new Queue<GameObject>();

                for (int i = 0; i < maxCount; i++)
                {
                    GameObject go = Instantiate(kvp.Value._prefab, backgroundLayer.TreadmillTransfrom);
                    go.layer = Layer;
                    go.name = kvp.Key;
                    GameObjectPool.Enqueue(go);
                }

                GoQueueByName.Add(kvp.Key, GameObjectPool);
            }

            foreach (var kvp in _backgroundLayerByName)
            {
                BackgroundLayer backgroundLayer = kvp.Value;
                List<string> backgroundsSequence = backgroundLayer.BackgroundsSequence;

                for (int i = 0; i < _activeBackgroundsLimit; i++)
                {
                    GameObject ActiveBackground = Spawn(backgroundsSequence[i]);

                    if (Level.IsVertical)
                        ActiveBackground.transform.position = new Vector3(0.0f, i * MainBackgroundPrefabHeight, backgroundLayer.TreadmillTransfrom.position.z);
                    else if (Level.IsHorizontal)
                        ActiveBackground.transform.position = new Vector3(i * MainBackgroundPrefabWidth, 0.0f, backgroundLayer.TreadmillTransfrom.position.z);

                    backgroundLayer.LastBackground = ActiveBackground;
                    backgroundLayer.CurrentSequenceIndex++;
                }
            }
        }

        public void ReplaceBackground(GameObject go)
        {
            if (go.layer == Layer)
            {
                float zPos = go.transform.position.z;
                if (!go.activeInHierarchy) return;

                Despawn(go);

                string layerName = StringTools.GetLayerName(go.name);
                BackgroundLayer backgroundLayer = _backgroundLayerByName[layerName];

                BackgroundLAayerEndAction(backgroundLayer);

                if (!backgroundLayer.CanCountinueSpawning) return;

                GameObject pulledBG = Spawn(backgroundLayer.BackgroundsSequence[backgroundLayer.CurrentSequenceIndex]);
                if (pulledBG == null) return;

                if (Level.IsVertical)
                    pulledBG.transform.position = new Vector3(0f, backgroundLayer.LastBackground.transform.position.y + MainBackgroundPrefabHeight, zPos);
                else if (Level.IsHorizontal)
                    pulledBG.transform.position = new Vector3(backgroundLayer.LastBackground.transform.position.x + MainBackgroundPrefabWidth, 0f, zPos);

                backgroundLayer.LastBackground = pulledBG;
                backgroundLayer.CurrentSequenceIndex++;
            }
        }

        private void BackgroundLAayerEndAction(BackgroundLayer backgroundLayer)
        {
            if (backgroundLayer.CurrentSequenceIndex >= backgroundLayer.BackgroundsSequence.Count)
            {
                switch (backgroundLayer.AfterSequenceEnds)
                {
                    case SequenceEndOptions.LoopFinallmage:
                        backgroundLayer.CurrentSequenceIndex = backgroundLayer.BackgroundsSequence.Count - 1;
                        break;
                    case SequenceEndOptions.LoopSequence:
                        backgroundLayer.CurrentSequenceIndex = 0;
                        break;
                    case SequenceEndOptions.StopAtFinallmage:
                        backgroundLayer.CanCountinueSpawning = false;
                        if (backgroundLayer.StopAtFinalImageCounter > 2)
                            backgroundLayer.StopAtFinalImageCounter--;
                        else
                            backgroundLayer.Speed = 0.0f;
                        break;
                    case SequenceEndOptions.FinishSequence:
                        backgroundLayer.CanCountinueSpawning = false;
                        break;
                }
            }
        }

        public GameObject Spawn(string goName)
        {
            if (!GoQueueByName.ContainsKey(goName))
            {
                Debug.Log($"Background Spawn: {goName} cannot be found.");
                return null;
            }

            if (GoQueueByName[goName].Count <= 0) return null;

            GameObject pulledGO = GoQueueByName[goName].Dequeue();
            if (pulledGO == null) return null;

            pulledGO.SetActive(true);

            string layerName = goName.Remove(ProjectConstants.LayerPrefixLength);
            BackgroundLayer backgroundLayer = _backgroundLayerByName[layerName];

            if (backgroundLayer.TreadmillTransfrom.gameObject.activeInHierarchy)
                pulledGO.transform.SetParent(backgroundLayer.TreadmillTransfrom);

            return pulledGO;
        }

        public void Despawn(GameObject go)
        {
            go.SetActive(false);

            try
            {
                GoQueueByName[go.name].Enqueue(go);
            }
            catch (KeyNotFoundException)
            {
                Debug.Log($"THE background Despawn: {go.name} can't find.");
            }

            string layerName = go.name.Remove(ProjectConstants.LayerPrefixLength);

            if (!_backgroundLayerByName.ContainsKey(layerName))
            {
                Debug.Log($"Can't find the background layer, {go.name} : {layerName}");
                return;
            }

            BackgroundLayer backgroundLayer = _backgroundLayerByName[layerName];
            Transform poolSubHierarchy = backgroundLayer.TreadmillTransfrom;

            if (poolSubHierarchy == null) return;
            if (poolSubHierarchy.gameObject.activeInHierarchy)
                go.transform.SetParent(poolSubHierarchy);
        }

        private bool CanProcessFirstBackground()
        {
            if (_backgroundLayers == null) return false;
            if (_backgroundLayers.Length < 1) return false;
            if (_backgroundLayers[0].Backgrounds.Length < 1) return false;
            if (_backgroundLayers[0].Backgrounds[0]._prefab == null) return false;
            return true;
        }

        public void CreatePreviewBackgrounds()
        {
            _previewBackground = new GameObject("----- Preview Backgrounds -----").transform;
            _previewBackground.parent = transform;

            for (int i = 0; i < _backgroundLayers.Length; i++)
            {
                int backgroundIndex = 0;
                for (int j = 0; j < _backgroundLayers[i].Backgrounds.Length; j++)
                {
                    for (int k = 0; k < _backgroundLayers[i].Backgrounds[j]._count; k++)
                    {
                        if (_backgroundLayers[i].Backgrounds[j]._prefab == null)
                        {
                            Debug.Log("ScrollingBackground.cs: missing background prefab.");
                            return;
                        }

                        GameObject previewBG = Instantiate(_backgroundLayers[i].Backgrounds[j]._prefab);
                        float zDepth = _backgroundLayers[i].DepthIndex * -Level.SpaceBetweenIndices;

                        if (Level.IsVertical)
                            previewBG.transform.position = new Vector3(0f, backgroundIndex * MainBackgroundPrefabHeight, zDepth);
                        else if (Level.IsHorizontal)
                            previewBG.transform.position = new Vector3(backgroundIndex * MainBackgroundPrefabWidth, 0f, zDepth);

                        previewBG.transform.SetParent(_previewBackground.transform);
                        backgroundIndex++;
                    }
                }
            }
        }

        public void DestroyPreviewBackgroundsInEditor()
        {
            if (_previewBackground != null)
                DestroyImmediate(_previewBackground.gameObject);
        }

        public void DestroyPreviewBackgrounds()
        {
            if (_previewBackground != null)
                Destroy(_previewBackground.gameObject);
        }
    }
}

