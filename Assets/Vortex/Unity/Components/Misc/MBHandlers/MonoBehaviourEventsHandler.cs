using UnityEngine;
using UnityEngine.Events;

public class MonoBehaviourEventsHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent onAwake = new();
    [SerializeField] private UnityEvent onDestroy = new();
    [SerializeField] private UnityEvent onEnable = new();
    [SerializeField] private UnityEvent onDisable = new();

    private void Awake() => onAwake?.Invoke();

    private void OnDestroy() => onDestroy?.Invoke();

    private void OnEnable() => onEnable?.Invoke();

    private void OnDisable() => onDisable?.Invoke();
}