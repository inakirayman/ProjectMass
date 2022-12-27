using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct CelestialBodyPreSet
{
    private float _hitboxSize;
    public float HitboxSize => _hitboxSize;

    private float _triggerRange;
    public float TriggerRange => _triggerRange;

    private float _minOrbitDistance;
    public float MinOrbitDistance => _minOrbitDistance;

    private float _minOrbitSpeed;
    public float MinOrbitSpeed => _minOrbitSpeed;

    private float _maxOrbitSpeed;
    public float MaxOrbitSpeed => _maxOrbitSpeed;

    private float _orbitSpacing;
    public float OrbitSpacing => _orbitSpacing;

    private float _orbitOffsetRange;
    public float OrbitOffsetRange => _orbitOffsetRange;

    private float _cooldown;
    public float Cooldown => _cooldown;

    private int _maxOrbitingObjects;
    public int MaxOrbitingObjects => _maxOrbitingObjects;

    private RuntimeAnimatorController _runtimeAnimator;

    public RuntimeAnimatorController RuntimeAnimator => _runtimeAnimator;

    private Vector3 _spriteScale;
    public Vector3 SpriteScale => _spriteScale;

    private float _mass;
    public float Mass => _mass;



   public CelestialBodyPreSet(GameObject gameObject, RuntimeAnimatorController runtimeAnimator)
   {
        _triggerRange = 0;
        _hitboxSize = 0;

        var colliders = gameObject.GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders)
            if (collider.isTrigger)
                _triggerRange = collider.radius;
            else
                _hitboxSize = collider.radius;

        var logic = gameObject.GetComponent<CelestialBodyLogic>();

        _minOrbitDistance = logic.MinOrbitDistance;
        _minOrbitSpeed = logic.MinOrbitSpeed;
        _maxOrbitSpeed = logic.MaxOrbitSpeed;
        _orbitSpacing = logic.OrbitSpacing;
        _orbitOffsetRange = logic.OrbitOffsetRange;
        _cooldown = logic.Cooldown;
        _maxOrbitingObjects = logic.MaxOrbitingObjects;

        _mass = gameObject.GetComponent<Rigidbody2D>().mass;

         _spriteScale = gameObject.transform.GetChild(0).transform.localScale;
        _runtimeAnimator = runtimeAnimator;
   }
}
