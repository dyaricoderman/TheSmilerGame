using UnityEngine;

public class AnimateOnView : MonoBehaviour
{
	public ParticleSystem PS;

	public void ARStart()
	{
		if (PS != null)
		{
			ParticleSystem.EmissionModule em = PS.emission;
			em.enabled = true;
			PS.Play();
		}
		base.gameObject.GetComponent<Animation>().Play();
	}

	public void ARStop()
	{
		if (PS != null)
		{
			ParticleSystem.EmissionModule em = PS.emission;
			em.enabled = true;
			PS.Clear();
			PS.Stop();
		}
		base.gameObject.GetComponent<Animation>().Rewind();
		base.gameObject.GetComponent<Animation>().Stop();
	}
}
