﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for players. Should only have as many instantiated as there are players.
/// </summary>
public class PlayerControls : Shooter {
    protected override bool ShouldShoot {
        get {
            Vector3 rayDirection = BulletSpawnPoint - transform.position;
            int notPlayerMask = ~(1 << LayerMask.NameToLayer("Player"));
            bool wallInWay = !Physics.Raycast(transform.position,
                                              rayDirection, 2, notPlayerMask);
            return Input.GetMouseButton(0) && wallInWay;
        }
    }

    protected override Vector3 Target {
        get {
            // Raycast to corresponding point on screen.
            Ray viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(viewRay, out hit);

            return CopyY(hit.point, Body.position);
        }
    }

    Vector3 inputAxes;
    Vector3 spawn;
    Camera follow;

    protected override void Start() {
        base.Start();

        inputAxes = new Vector3(0, 0, 0);
        follow = Camera.main;
        spawn = transform.position;
    }

    protected override void Update() {
        base.Update();
       
        UpdateInputAxes();
        TrackCamera();
    }

    void UpdateInputAxes() {
        inputAxes.x = Input.GetAxis("Horizontal");
        //inputAxes.y;
        inputAxes.z = Input.GetAxis("Vertical");

		this.transform.rotation = Quaternion.LookRotation (new Vector3 (inputAxes.x, inputAxes.y, 0.0f));

        inputAxes *= Speed;
		this.transform.rotation = Quaternion.LookRotation (new Vector3 (inputAxes.x, inputAxes.y, 0.0f));
    }

    void TrackCamera() {
        follow.transform.position = CopyY(transform.position,
                                          follow.transform.position);
    }

    void FixedUpdate() {
        Body.velocity = CopyY(inputAxes, Body.velocity);
    }

    /// <summary>
    /// Creates a copy of v with w.y substituted.
    /// </summary>
    /// <returns>"Copied" vector.</returns>
    /// <param name="to">Destination.</param>
    /// <param name="from">Source.</param>
    Vector3 CopyY(Vector3 to, Vector3 from) {
        to.y = from.y;
        return to;
    }

    void OnTriggerEnter(Collider other) {
        string otherTag = other.gameObject.tag;

        if (otherTag == "Health Pickup") {
            CollectHealth();
            Destroy(other.gameObject);
        } else if (otherTag == "Shield Pickup") {
            CollectShield();
            Destroy(other.gameObject);
        } else if (otherTag == "Ammo Pickup") {
            CollectAmmo();
            Destroy(other.gameObject);
        }
    }

    void CollectHealth() {
        Health.Heal(GameManager.Instance.baseHealAmount * GameManager.Instance.healMultiplier);
    }

    public GameObject shield;

    void CollectShield() {
        Destroy(Instantiate(shield, transform), GameManager.Instance.shieldActiveTime);
    }

    void CollectAmmo() {
        Debug.Log("Collected ammo pickup");
    }

    protected override void Die() {
        Health.Reset();

        // also, UpdateScore();
        transform.position = spawn;
    }
}
