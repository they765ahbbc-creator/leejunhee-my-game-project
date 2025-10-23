using System.Collections.Generic;
using UnityEngine;

namespace ShootemUp
{
    [System.Serializable]
    public class BackgroundLayer
    {
        public string LayerName;
        public GoCount[] Backgrounds;
        public float Speed;
        public SequenceEndOptions AfterSequenceEnds;

        [Range(0, ProjectConstants.DepthIndexLimit)]
        public int DepthIndex;

        public int CurrentSequenceIndex {  get; set; }
        public Transform PoolTransfrom {  get; set; }
        public Transform TreadmillTransfrom { get; set; }
        public GameObject LastBackground { get; set; }
        public int StopAtFinalImageCounter {  get; set; }
        public bool CanCountinueSpawning {  get; set; }

        [HideInInspector]
        public List<string> BackgroundsSequence = new List<string>();
    }
}
