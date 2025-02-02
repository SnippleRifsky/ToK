using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NameplateManager : MonoBehaviour
{
        private Canvas _canvas;
        
        [SerializeField]
        public GameObject uiEntityNameplatePrefab;
        private readonly Queue<UIEntityNameplate> _nameplatePool = new();
        private readonly Dictionary<Entity, UIEntityNameplate> _activeNameplates = new();
        
        public void Initialize()
        {
                _canvas = UIManager.Instance.UICanvas;
        }
        
        public void ShowEntityNameplate(Entity entity)
        {
                if (_activeNameplates.ContainsKey(entity)) return;

                var nameplate = GetNameplateFromPool();
                nameplate.Setup(entity);
                UpdateNameplatePosition(nameplate, entity);
                _activeNameplates[entity] = nameplate;
        }
        
        public void HideEntityNameplate(Entity entity)
        {
                if (_activeNameplates.TryGetValue(entity, out var nameplate))
                {
                        ReturnNameplateToPool(nameplate);
                        _activeNameplates.Remove(entity);
                }
        }
        private UIEntityNameplate GetNameplateFromPool()
        {
                if (_nameplatePool.Count > 0) return _nameplatePool.Dequeue();

                var go = Instantiate(uiEntityNameplatePrefab, _canvas.transform).GetComponent<UIEntityNameplate>();
                return go;
        }
        private void ReturnNameplateToPool(UIEntityNameplate nameplate)
        {
                nameplate.Clear();
                nameplate.gameObject.SetActive(false);
                _nameplatePool.Enqueue(nameplate);
        }
        
        private void UpdateNameplatePosition(UIEntityNameplate nameplate, Entity entity)
        {
                var worldPosition = entity.transform.position;

                var collider = nameplate.GetCachedCollider();
                if (collider is null) return;
                worldPosition.y += collider.bounds.extents.y;

                var screenPosition = GameManager.Instance.PlayerCamera.WorldToScreenPoint(worldPosition);
                nameplate.transform.position = screenPosition;
                nameplate.gameObject.SetActive(screenPosition.z > 0);
        }
        
        private void Update()
        {
                foreach (var kvp in _activeNameplates)
                {
                        var entity = kvp.Key;
                        var nameplate = kvp.Value;
                        UpdateNameplatePosition(nameplate, entity);
                }
        }
}