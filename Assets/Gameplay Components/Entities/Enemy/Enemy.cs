public class Enemy : Entity
{
    private UIEntityNameplate _entityNameplate;

    private void Start()
    {
        _entityNameplate = gameObject.AddComponent<UIEntityNameplate>();
    }
}
