using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedProjectile : MonoBehaviour
{
    public ProjectileEnum.BEHAVIOR_TYPE GET_BEHAVIOR;
    public ProjectileEnum.MOVE_TYPE GET_MOVEMENT;
    public float SET_PROJECTILE_SPEED = 10f;
    // [HideInInspector]
    ProjectileObjectPooler m_POP;

    void Start()
    {
        m_POP = transform.root.GetComponentInChildren<ProjectileObjectPooler>();
    }

    public void SetProjectileBehaviorByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        GET_BEHAVIOR = (ProjectileEnum.BEHAVIOR_TYPE)ParsedClickButtonName;

        SetProjectileInProjectileManager();
    }

    public void SetProjectileMovementByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        GET_MOVEMENT = (ProjectileEnum.MOVE_TYPE)ParsedClickButtonName;

        SetProjectileInProjectileManager();
    }

    void SetProjectileInProjectileManager()
    {
        if (m_POP == null) return;
        if (m_POP.projectileList.Count <= 0) return;

        foreach (var projectile in m_POP.projectileList) 
        {
            projectile.GetComponent<ProjectileManager>().GET_BEHAVIOR = GET_BEHAVIOR;
            projectile.GetComponent<ProjectileManager>().GET_MOVEMENT = GET_MOVEMENT;
            projectile.GetComponent<ProjectileHover>().driveForce = SET_PROJECTILE_SPEED;
        }
    }
}
