using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolveHelper : MonoBehaviour
{
    public static int PlanetMass = 20;
    public static int StarMass = 40;
    public static int BlackHoleMass = 60;



    [Header("Astroid")]
    [SerializeField] private GameObject _astroidBase;
    private static CelestialBodyPreSet _astroid;

    [Header("Planet")]
    [SerializeField] private GameObject _planetBase;
    private static CelestialBodyPreSet _planet;

    [Header("Star")]
    [SerializeField] private GameObject _starBase;
    private static CelestialBodyPreSet _star;

    [Header("BlackHole")]
    [SerializeField] private GameObject _blackHoleBase;
    private static CelestialBodyPreSet _blackhole;

    [Header("Animator Controllers")]
    [SerializeField] private RuntimeAnimatorController _astroidAnimatorControllers;
    [SerializeField] private RuntimeAnimatorController _planetAnimatorControllers;
    [SerializeField] private RuntimeAnimatorController _starAnimatorControllers;
    [SerializeField] private RuntimeAnimatorController _blackHoleAnimatorControllers;



    // Start is called before the first frame update
    void Start()
    {
        _astroid = new CelestialBodyPreSet(_astroidBase, _astroidAnimatorControllers);
        _planet = new CelestialBodyPreSet(_planetBase, _planetAnimatorControllers);
        _star = new CelestialBodyPreSet(_starBase, _starAnimatorControllers);
        _blackhole = new CelestialBodyPreSet(_blackHoleBase, _blackHoleAnimatorControllers);
    }



    public static void UpdateStats(GameObject gameObject, CelestialBodyType type)
    {
        if (type == CelestialBodyType.Astroid)
            SetObjectSettings(gameObject, _astroid);

        else if (type == CelestialBodyType.Planet)
            SetObjectSettings(gameObject, _planet);

        else if (type == CelestialBodyType.Star)
            SetObjectSettings(gameObject, _star);

        else if (type == CelestialBodyType.Blackhole)
            SetObjectSettings(gameObject, _blackhole);
    }

    private static void SetObjectSettings(GameObject gameObject, CelestialBodyPreSet Preset)
    {
        var colliders = gameObject.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
            if (collider.isTrigger)
                collider.radius = Preset.TriggerRange;
            else
                collider.radius = Preset.HitboxSize;

        var logic = gameObject.GetComponent<CelestialBodyLogic>();

        logic.MinOrbitDistance = Preset.MinOrbitDistance;
        logic.MinOrbitSpeed = Preset.MinOrbitSpeed;
        logic.MaxOrbitSpeed = Preset.MaxOrbitSpeed;
        logic.OrbitSpacing = Preset.OrbitSpacing;
        logic.OrbitOffsetRange = Preset.OrbitOffsetRange;
        logic.Cooldown = Preset.Cooldown;
        logic.MaxOrbitingObjects = Preset.MaxOrbitingObjects;

        gameObject.GetComponent<Rigidbody2D>().mass = Preset.Mass;

        gameObject.transform.GetChild(0).transform.localScale = Preset.SpriteScale;
        gameObject.GetComponentInChildren<Animator>().runtimeAnimatorController = Preset.RuntimeAnimator;
        
    }
}
