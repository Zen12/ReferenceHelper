using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ReferenceHelper.Editor
{
    public class ReferenceHelper : EditorWindow
    {
        [MenuItem("Tools/ReferenceHelper")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ReferenceHelper));
        }

        private void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;
        }
        
        private void OnDisable()
        {
            try
            {
                Selection.selectionChanged -= SelectionChanged;
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
                throw;
            }
        }

        private void SelectionChanged()
        {
            Repaint();
        }

        void OnGUI()
        {
            var obj = Selection.activeObject;
            if (obj != null && obj is GameObject)
            {
                FindReferences((GameObject) obj);
            }
        }

        private void FindReferences(GameObject obj)
        {
            var comps = obj.GetComponents<Component>();

            var listGO = new List<GameObject>();
            
            foreach (var c in comps)
            {
                var (cc, co) = FindReferencesInSceneForComponents(c);
                foreach (var o in co)
                {
                    if (listGO.Contains(o) == false)
                    {
                        listGO.Add(o);
                    }
                }
                
                if (cc.Count > 0)
                {
                    GUILayout.Label(c.GetType().ToString());
                }

                foreach (var component in cc)
                {
                    if (GUILayout.Button(component.ToString()))
                    {
                        Selection.activeObject = component;
                        EditorGUIUtility.PingObject(component);
                    }
                }
            }
            
                            
            if (listGO.Count > 0)
            {
                GUILayout.Label(listGO[0].GetType().ToString());
            }

            foreach (var component in listGO)
            {
                if (GUILayout.Button(component.ToString()))
                {
                    Selection.activeObject = component;
                    EditorGUIUtility.PingObject(component);
                }
            }
        }

        private (List<Component>, List<GameObject>) FindReferencesInSceneForComponents(Component component)
        {
            var list = new List<Component>();
            var listGO = new List<GameObject>();
            if (component == null)
                return (list, listGO);
            
            var scene = component.gameObject.scene;
            var roots = scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                var gs = root.GetComponentsInChildren<Component>(true);
                foreach (var g in gs)
                {
                    if (g == null)
                        continue;
                    
                    //fields
                    var fields = g.GetType().GetFields();
                    foreach (var field in fields)
                    {
                        if (field.FieldType == component.GetType())
                        {
                            var val = field.GetValue(g);
                            if (val != null)
                            {
                                if (component.Equals(val))
                                {
                                    list.Add(g);
                                }
                            }
                        } else if (field.FieldType == typeof(GameObject))
                        {
                            var val = field.GetValue(g);
                            if (val != null)
                            {
                                if (component.gameObject.Equals(val))
                                {
                                    listGO.Add(g.gameObject);
                                }
                            }
                        }
                        else if (field.FieldType == typeof(UnityEvent))
                        {
                            var ev = (UnityEvent) field.GetValue(g);
                            var index = ev.GetPersistentEventCount();
                            for (int i = 0; i < index; i++)
                            {
                                var target = ev.GetPersistentTarget(i);
                                if (target != null)
                                {
                                    if (target.Equals(component))
                                    {
                                        list.Add(g);
                                    }
                                }
                            }
                        }
                    }
                    
                    //events
                    if (g.GetType() == typeof(EventTrigger))
                    {
                        var ev = (EventTrigger) g;
                        var triggers = ev.triggers;
                        foreach (var trigger in triggers)
                        {
                            var ev2 = trigger.callback;
                            var index = ev2.GetPersistentEventCount();
                            for (int i = 0; i < index; i++)
                            {
                                var target = ev2.GetPersistentTarget(i);
                                if (target.Equals(component))
                                {
                                    list.Add(g);
                                }
                            }
                        }
                    }
                }
            }

            return (list, listGO);
        }
    }
}
