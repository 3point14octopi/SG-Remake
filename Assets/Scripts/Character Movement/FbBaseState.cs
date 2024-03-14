using UnityEngine;

public abstract class FbBaseState
{
    //Used when a new state is entered
   public abstract void EnterState(FbStateManager fb);

    //Called every mono update while in that state
   public abstract void UpdateState(FbStateManager fb);

    //Used for our physics interactions
   public abstract void FixedUpdateState(FbStateManager fb);

    //Called when a collision happens while in that state
   public abstract void Collision(FbStateManager fb, Collision2D Collision2D);
   
}
