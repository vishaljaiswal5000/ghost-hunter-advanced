using UnityEngine;

// Kept so the scene's component reference stays valid.
// The wreck sound is played by UpdateScore when the car is hit.
public class CarAnimator : MonoBehaviour
{
    [SerializeField] private AudioSource dead;
    [SerializeField] private AudioSource collide;
    [SerializeField] private AudioSource normalPelletEaten;
    [SerializeField] private AudioSource powerPelletEaten;
    [SerializeField] private AudioSource bonusPelletEaten;
}
