using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemyController : MonoBehaviour
{
	public float health = 100.0f;
	public Image healthBar;
	[HideInInspector] public Rigidbody rb;
	public float speed = 10.0f;
	[HideInInspector] public List<Collider> hurtboxes = new List<Collider>();
	public Hurtbox hurtbox;

	protected State currentState;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		healthBar.fillAmount = health / 100.0f;
	}

	void FixedUpdate()
	{
		currentState = currentState?.RunCurrentState();
	}

	public void Damage(float damage)
	{
		health -= damage;
		Debug.Log(health);
	}
}
