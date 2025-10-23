using UnityEngine;

namespace ShootemUp
{
    public static class TransformTools
    {
    public static void ResetParentPosition(Transform hierarchy)
        {
            int childCount = hierarchy.childCount;
            Transform[] children = new Transform[childCount]; 

            for(int i = 0; i < childCount; i++)
            {
                children[i] = hierarchy.GetChild(i);
            }

            foreach(Transform child in children)
            {
                child.transform.SetParent(null);
            }

            hierarchy.position = Vector3.zero;

            foreach(Transform child in children)
            {
                child.transform.SetParent(hierarchy);
            }
        }
    }
}
