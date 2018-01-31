using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace namudev
{
    public class PropertyGrid : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        public object targetObject;
        // private GameObject targetObject;

        [SerializeField]
        private bool logging;
#pragma warning restore 649

        private GameObject label;
        private GameObject scrollbar;

        private Dictionary<string, GameObject> itemTemplateMap;

        private List<GameObject> items;

#if UNITY_EDITOR
        [MenuItem("GameObject/UI/PropertyGrid")]
        private static void Create()
        {
            string[] assets = AssetDatabase.FindAssets("l:NamudevPropertyGrid");
            if (assets.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                GameObject gameObject = Instantiate(prefab);
                gameObject.name = prefab.name;
                Transform activeTransform = Selection.activeTransform;
                if ((activeTransform != null) && (activeTransform.gameObject.GetComponent<RectTransform>() != null))
                {
                    gameObject.transform.SetParent(activeTransform, false);
                }
                else
                {
                    var canvas = FindObjectOfType<Canvas>();
                    if (canvas != null)
                    {
                        gameObject.transform.SetParent(canvas.transform, false);
                    }
                }
                Undo.RegisterCreatedObjectUndo(gameObject, "Create PropertyGrid");
            }
            else
            {
                Debug.LogError("PropertyGrid prefab not found");
            }
        }
#endif

        public void AppendLabel(string text)
        {
            GameObject item = Instantiate(label);
            items.Add(item);

            item.name = text;
            item.transform.SetParent(label.transform.parent);
            item.SetActive(true);
            item.GetComponentInChildren<Text>().text = text;
        }

        public T AppendProperty<T>(string caption, object value)
            where T : Component
        {
            if (!PropertyGridItem.TypeMap.ContainsValue(typeof(T)))
            {
                string format = "Can not add property of type {0}";
                string message = string.Format(format, typeof(T).Name);
                Log(message);
                return default(T);
            }

            Type type = PropertyGridItem.TypeMap.FirstOrDefault(pair => pair.Value == typeof(T)).Key;
            if (!itemTemplateMap.ContainsKey(type.FullName))
            {
                string format = "No template found for type {0}";
                string message = string.Format(format, type.Name);
                Log(message);
                return default(T);
            }

            GameObject item = Instantiate(itemTemplateMap[type.FullName]);
            items.Add(item);

            PropertyGridBinding binding = item.AddComponent<PropertyGridBinding>();
            binding.Initialize(caption, value, type);

            item.name = caption;
            item.transform.SetParent(itemTemplateMap[type.FullName].transform.parent);
            item.AddComponent<T>();
            item.SetActive(true);

            return item.GetComponent<T>();
        }

        public void Populate(object obj)
        {
            var gameObject = obj as GameObject;
            if (gameObject != null)
            {
                AppendLabel(gameObject.GetType().Name);
            }
            AppendProperties(obj);
            if (gameObject != null)
            {
                Component[] components = gameObject.GetComponents<Component>();
                foreach (Component component in components)
                {
                    AppendLabel(component.GetType().Name);
                    var exclude = new List<string> { "hideFlags", "name", "tag" };
                    AppendProperties(component, exclude);
                }
            }
        }

        public void Clear()
        {
            scrollbar.GetComponent<Scrollbar>().value = 1.0f;
            foreach (GameObject item in items)
            {
                Destroy(item);
            }
            items.Clear();
        }

        private void Awake()
        {
            label = transform.Find("Scroll/Panel/Label").gameObject;
            scrollbar = transform.Find("Scrollbar").gameObject;
            itemTemplateMap = new Dictionary<string, GameObject>();
            foreach (Transform t in transform.Find("Scroll/Panel"))
            {
                var template = t.gameObject.GetComponent<PropertyGridTemplate>();
                if (template != null)
                {
                    string typeStr = template.Type;
                    if (!string.IsNullOrEmpty(typeStr))
                    {
                        itemTemplateMap[typeStr] = t.gameObject;
                        string format = "Matched template '{0}' with type '{1}' ";
                        string message = string.Format(format, template.gameObject.name, typeStr);
                        Log(message);
                    }
                }
                t.gameObject.SetActive(false);
            }
            items = new List<GameObject>();

            if (targetObject != null)
            {
                Populate(targetObject);
            }
        }

        private void AppendProperties(object obj, List<string> exclude = null)
        {
            List<PropertyData> propertyList = obj as List<PropertyData>;
            if (null != propertyList)
            {
                foreach (PropertyData pp in propertyList)
                {
                    AppendProperty(obj, pp);
                }
            }
        }

        private void AppendProperty(object obj, PropertyData propertyInfo)
        {
            Type key = null;
            if (itemTemplateMap.ContainsKey(propertyInfo.PropertyType.FullName))
            {
                key = propertyInfo.PropertyType;
            }
            else if (propertyInfo.PropertyType.IsSubclassOf(typeof(Enum)))
            {
                key = typeof(Enum);
            }
            if (key == null)
            {
                string format = "Skipped property '{0}' (no template found for type '{1}')";
                string message = string.Format(format, propertyInfo.PropertyName, propertyInfo.PropertyType.Name);
                Log(message);
                return;
            }

            GameObject item = Instantiate(itemTemplateMap[key.FullName]);
            items.Add(item);

            PropertyGridBinding binding = item.AddComponent<PropertyGridBinding>();
            binding.Initialize(obj, propertyInfo);

            item.name = propertyInfo.PropertyName;
            item.transform.SetParent(itemTemplateMap[key.FullName].transform.parent);
            item.AddComponent(PropertyGridItem.TypeMap[key]);
            item.SetActive(true);
        }

        private void Log(string message)
        {
            if (logging)
            {
                Debug.Log("[PROPERTYGRID] " + message);
            }
        }

        public Canvas m_canvas;
        public Camera m_camera;
        void Update()
        {
            //float camHeight = m_camera.orthographicSize *2f;
            Vector3 camTopRight =  new Vector3(m_camera.pixelWidth * 0.5f, m_camera.pixelHeight * 0.5f);
            Vector3 topRight = Utils.Cam2PixCoord(m_camera.transform.position, m_camera, m_canvas) + camTopRight;

            RectTransform rectTrf = GetComponent<RectTransform>();
            rectTrf.localPosition = camTopRight - (Vector3)Utils.Cam2PixCoord(rectTrf.sizeDelta * 0.5f, m_camera, m_canvas);

            // Debug.Log(rectTrf.position.ToString());
        }
    }
}
