using EntityStats;
using UnityEngine;
using UpgradeStats;

public class FbBrain : Brain
{
    [Tooltip("Gun manager reference")]
    public FbGun gun;

    [Tooltip("State manager reference")]
    public FbStateManager stateManager;

    [Tooltip("these are the highest values stats go up to. Ex. Max possible health")]
    public float[] maxStats = new float[3]; 

    [Header("\nRunning")]
    public Vector2 movement;
    public Rigidbody2D rb; //player rigidbody
    public bool iFrame = false;

    public Animator anim;
    public GameObject healthbar;
    public GameObject iceBar;

    //Variables for upgrades
    private bool allowHealth = true;
    private bool bleedingHearts = false;

    // Start is called before the first frame update
    private void Start()
    {
        //sets our rigidbody reference
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        foreach (Reaction r in damageReactions) r.OnStart(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //tracks WASD
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

    }

    //called by Moving state on update || Handles moving our player and our movement animations
    public void Moving()
    {
        //If we are moving in both x and y (on a diagonal) we move at a reduced speed
        if(movement.x != 0 && movement.y != 0) rb.MovePosition(rb.position + movement * currentStats[1] * Time.fixedDeltaTime * 0.72f);
        //if not on a diagonal we can move at full speed
        else rb.MovePosition(rb.position + movement * currentStats[1] * Time.fixedDeltaTime);

        //handles our animation for moving
        if (movement.x == 1) { anim.Play("FrostbiteWalkRight"); }
        else if (movement.x == -1) { anim.Play("FrostbiteWalkLeft"); }
        else if (movement.x == 0 && movement.y == 1) { anim.Play("FrostbiteWalkUp"); }
        else if (movement.x == 0 && movement.y == -1)
        {
            anim.Play("FrostbiteWalkDown");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageTags.Contains(collision.gameObject.tag) && !iFrame)
        {
            foreach (HitEffect effect in collision.gameObject.GetComponent<OnHit>().effects) OnHit(effect);
            if (currentStats[(int)EntityStat.Health] <= 0)
            {
                stateManager.SwitchState(stateManager.DeathState);
            }
            else
            {
                ReactToHit();
            }
        }
    }

    //applies the effects of and object we ran into. Currently needs to be a switch(or lookup I guess) cause of things like the healthbar being updated
    private void OnHit(HitEffect effect)
    {
        switch (effect.targetedStat)
        {
            case EntityStat.Health:
                {
                    if (currentStats[(int)effect.targetedStat] + effect.modifier <= Stats[(int)effect.targetedStat]){
                        currentStats[(int)effect.targetedStat] += effect.modifier;
                        healthbar.GetComponent<FbHealthBar>().HealthBar(currentStats[(int)effect.targetedStat]);
                        if (bleedingHearts) gun.currentAmmo.bullet.bulletEffects[(int)EntityStat.Health] = new HitEffect(EntityStat.Health, gun.rawDamage + gun.rawDamage * (0.25f * (Stats[0] - currentStats[0])));
                    }
                    break;
                }
            case EntityStat.Speed:
                {
                    if (currentStats[(int)effect.targetedStat] + effect.modifier <= Stats[(int)effect.targetedStat]){
                        currentStats[(int)effect.targetedStat] += effect.modifier;
                    }
                    break;
                }

        }
    }
    //given  by upgrade manager lets us vary any of our entity states based on a upgrade pickup. Currently needs to be a switch(or lookup I guess) cause of things like the healthbar being updated
    public void PlayerUpgrade(PlayerUpgrade upgrade)
    {
        switch (upgrade.playerUpgrade)
        {
            case PlayerUpgrades.Health:
 
                if (currentStats[0] + upgrade.modifier <= Stats[0] && allowHealth)
                { 
                    currentStats[0] += upgrade.modifier;
                    if (bleedingHearts) gun.currentAmmo.bullet.bulletEffects[(int)EntityStat.Health] = new HitEffect(EntityStat.Health, gun.rawDamage + gun.rawDamage * (0.25f * (Stats[0] - currentStats[0])));
                    healthbar.GetComponent<FbHealthBar>().HealthBar(currentStats[0]);
                }
            break;

            case PlayerUpgrades.MaxHealth:
                if (Stats[0] + upgrade.modifier <= maxStats[0] && allowHealth)
                {
                    Stats[0] += upgrade.modifier;
                    currentStats[0] += upgrade.modifier; if (bleedingHearts) gun.currentAmmo.bullet.bulletEffects[(int)EntityStat.Health] = new HitEffect(EntityStat.Health, gun.rawDamage + gun.rawDamage * (0.25f * (Stats[0] - currentStats[0])));

                    if (upgrade.modifier > 0)
                    {
                        healthbar.GetComponent<FbHealthBar>().IncreaseHealthBar(Stats[0], currentStats[0]);
                    }
                    else if (upgrade.modifier < 0)
                    {
                        healthbar.GetComponent<FbHealthBar>().DecreaseHealthBar(Stats[0], currentStats[0]);
                    }

                }
                else if (Stats[0] < maxStats[0]) Stats[0] = maxStats[0];
            break;


            case PlayerUpgrades.Speed:
                if (Stats[1] + upgrade.modifier <= maxStats[1]) currentStats[1] += upgrade.modifier;
                else if (Stats[1] < maxStats[1]) Stats[1] = maxStats[1];
            break;

            case PlayerUpgrades.NoRiskNoReward:
                if (currentStats[0] > 2) currentStats[0] = 2;
                healthbar.GetComponent<FbHealthBar>().DecreaseHealthBar(2, currentStats[0]);
                allowHealth = false;
            break;

            case PlayerUpgrades.BleedingHearts:
                bleedingHearts = true;
            break;

        }
    }

}
