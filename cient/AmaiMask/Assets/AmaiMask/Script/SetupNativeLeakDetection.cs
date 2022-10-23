using UnityEngine;
using Unity.Collections;

public class SetupNativeLeakDetection : MonoBehaviour
{
    void Start()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
    }
}
