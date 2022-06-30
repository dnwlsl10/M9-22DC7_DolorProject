using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;
using UnityEngine.XR;
public enum ActionMap{XRI_Head, XRI_LeftHand, XRI_LeftHand_Interaction, XRI_LeftHand_Locomotion, XRI_RightHand, XRI_RightHand_Interaction, XRI_RightHand_Locomotion}

public class Utility : MonoBehaviour
{
    public static bool cache;
    public static bool initialized;
    public static bool isVRConnected
    {
        get 
        {
            if (initialized == false)
            {
                List<XRDisplaySubsystem> lists = new List<XRDisplaySubsystem>();
                SubsystemManager.GetInstances<XRDisplaySubsystem>(lists);
                foreach (var display in lists)
                    if (display.running)
                    {
                        cache = true;
                        initialized = true;
                        Debug.Log("VR Connected");
                        return true;
                    }
                
                cache = false;
                initialized = true;
                Debug.Log("VR Not Connected");
                return false;
            }
            else
                return cache;
        }
    }

    public static Transform FindChildMatchName(Transform tr, string name)
    { return FindChildMatchName(tr, new string[]{name}); }
    public static Transform FindChildMatchName(Transform tr, string[] names)
    {
        

        for (int i = 0; i < tr.childCount; i++)
        {
            Transform tmp = tr.GetChild(i);
            foreach (var name in names)
                if (tmp.name == name)
                    return tmp;
            
            var ret = FindChildMatchName(tmp, names);
            if (ret != null) return ret;
        }
        return null;
    }

    public static Transform FindChildContainsName(Transform tr, string name)
    { return FindChildContainsName(tr, new string[]{name}); }
    public static Transform FindChildContainsName(Transform tr, string[] names)
    {
        if (tr == null)
        { Debug.LogWarning("Transform reference is null"); return null;}
        
        for (int i = 0; i < tr.childCount; i++)
        {
            Transform tmp = tr.GetChild(i);
            foreach (var name in names)
                if (tmp.name.Contains(name))
                    return tmp;

            var ret = FindChildContainsName(tmp, names);
            if (ret != null) return ret;
        }
        return null;
    }

    public static void ExecuteMethod(MonoBehaviour mb, string methodName, params object[] parameters)
    {
        if (mb == null)
        {
            Debug.LogWarning("MonoBehaviour is null");
            return;
        }
        System.Type type = mb.GetType();
        MethodInfo mi = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
        if (mi == null)
        {
            mi = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi == null)
            {
                Debug.LogWarning(methodName + " Not Found... Is this Static Method?");
                return;
            }
        }
        print(mi.Name);
                
        ParameterInfo[] arguments = mi.GetParameters();
        if (parameters.Length == arguments.Length)
            mi.Invoke(mb, parameters);
        else
        {
            Debug.LogWarning("Parameter Count Doesn't Match");
            return;
        }
    }

    public static void GetBoneTransform(Transform root, HumanBodyBones boneName, out Transform boneTransform)
    {
        Animator anim = root.GetComponent<Animator>();
        if (anim == null || anim.isHuman == false)
        {
            Debug.LogError("Needs Humanoid Avatar");
            boneTransform = null;
            return;
        }
        boneTransform = anim.GetBoneTransform(boneName);
        
        return;
    }
    #if UNITY_EDITOR
    public static InputActionReference FindInputReference(ActionMap actionMap, string action)
    {
        var references = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/3.Util/XR/Samples/XR Interaction Toolkit/2.0.2/Starter Assets/XRI Default Input Actions.inputactions");
        foreach (var reference in references)
            if (reference.name == actionMap.ToString().Replace('_', ' ')+"/"+action)
                return reference as InputActionReference;
        return null;
    }

    public static InputAction CloneAction(ActionMap actionMap, string action)
    {
        var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/3.Util/XR/Samples/XR Interaction Toolkit/2.0.2/Starter Assets/XRI Default Input Actions.inputactions");
        var map = asset.actionMaps[(int)actionMap];
        InputAction tmp=null;
        foreach (var a in map.actions)
            if (a.name == action)
                tmp = a;

        print(tmp);
        
        InputAction result = new InputAction(actionMap.ToString().Replace('_', ' ')+"/"+action, tmp.type, null, tmp.interactions, tmp.processors, tmp.expectedControlType);
        foreach (var v in tmp.bindings)
            result.AddBinding(v);

        return result;
    }

    public static T Load<T>(string path) where T : Object
    {
         return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
    }
    #endif
}