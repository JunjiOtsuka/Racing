public class ProjectileSimpleFactory
{
    //Projectile Behavior
    public IProjectileBehavior mPB;

    //Projectile Movement
    public IProjectileMovement mPM;

    public ProjectileEnum.BEHAVIOR_TYPE GET_BEHAVIOR;
    public ProjectileEnum.MOVE_TYPE GET_MOVEMENT;

    public ProjectileTypeInitializer mPTI;
    public ProjectileSimpleFactory mPSF;

    public ProjectileSimpleFactory(IProjectileBehavior newmPB, IProjectileMovement newmPM, ProjectileTypeInitializer newmPTI)
    {
        //grab the list of cached/initialized projectile types
        this.mPTI = newmPTI;

        //setters for projectile movement and behavior
        this.mPB = newmPB;
        this.mPM = newmPM;
    }

    public IProjectileBehavior CheckBehavior(ProjectileEnum.BEHAVIOR_TYPE GET_BEHAVIOR)
    {
        switch(GET_BEHAVIOR)
        {
            case ProjectileEnum.BEHAVIOR_TYPE.NORMAL:
                mPB = mPTI.TYPE_B_NORMAL;
                break;
            case ProjectileEnum.BEHAVIOR_TYPE.HOMING:
                mPB = mPTI.TYPE_B_HOMING;
                break;
            //add more behavior type here
                //...



        }
        return mPB;
    }

    public IProjectileMovement CheckMovement(ProjectileEnum.MOVE_TYPE GET_MOVEMENT)
    {
        switch(GET_MOVEMENT)
        {
            case ProjectileEnum.MOVE_TYPE.NORMAL:
                mPM = mPTI.TYPE_M_NORMAL;
                break;
            case ProjectileEnum.MOVE_TYPE.WAVE:
                mPM = mPTI.TYPE_M_WAVE;
                break;
            //add more move type here
                //...



        }
        return mPM;
    }
}
