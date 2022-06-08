using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor_Beam : MonoBehaviour
{
   
    GameObject targetObject = null;

    public ParticleSystem tractor_Beams;
    public ParticleSystem tractor_Beam_Particles;
    public ParticleSystem tractor_Beam_Rings;

    public AudioSource tractorBeamAudio;
    public AudioSource tractorBeamLockedAudio;

    public GameObject beamSource;
    
    public float beamAngle;

    [Space(5)]
    [Header("Track target (even if it is out of range)")]
    public bool trackTarget = false;
    public GameObject beamTarget;
    [Space(10)]

    public float rotationSpeed = 1.0f;
    public float beamPower = 5;

    bool insideTractorBeam = false;


    // Use this for initialization
    void Start()
    {

        targetObject = beamTarget;

        var beam_shape = tractor_Beams.shape;
        var beam_particles =  tractor_Beam_Particles.shape;

        beam_shape.angle = beamAngle;
        beam_particles.angle = beamAngle;

        tractorBeamAudio.Play();

    }


    // Update is called once per frame
    void Update()
    {

        // Beam tracking - Get position of target object
        Vector3 targetPosition = targetObject.transform.position;

        // Beam tracking - Ccalculate rotation to be done
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);


        // Beam tracking - Apply rotation
        if (trackTarget)
        { 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        // Apply beam force to target

        if (insideTractorBeam)
        {

            beamTarget.transform.position += (beamSource.transform.position - beamTarget.transform.position).normalized * (Time.deltaTime * beamPower);

        }


    }


    void OnTriggerEnter (Collider other)
    {

        

        insideTractorBeam = true;
        tractorBeamLockedAudio.Play();
        tractorBeamAudio.Stop();

    }


    void OnTriggerExit(Collider other)
    {

        // beamTarget.GetComponent<Rigidbody>().Sleep();

        insideTractorBeam = false;

        tractorBeamLockedAudio.Stop();
        tractorBeamAudio.Play();

    }
    

}