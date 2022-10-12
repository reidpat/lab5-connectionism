using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class RollerAgent : Agent
{
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>(); 
    }

    public Transform Target;

    // Set up the new environment for a new episode
    public override void OnEpisodeBegin(){
        if (this.transform.localPosition.y < 0){
            //if agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.rBody.transform.localPosition = new Vector3(0,0.5f, 0);
        }

        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    // Get relevant information from the environment to effectively learn behavior
    public override void CollectObservations(VectorSensor sensor){
        //Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        //agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

    }

    public float forceMultiplier = 10;
    // What to do when an action is received (i.e. when the Brain gives the agent information about possible actions)
    public override void OnActionReceived(float[] vectorAction){

        // Apply 2 actions to x and z coordinates of the ball (to move on the platform)
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // distance between the agent (the ball) and the target
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        // TODO: determine when to give reward to the agent, the amount of reward,
        // and what happens after reward is given
        // Hint: check the Agents documentation for relevant functions to use
        
        


        // the local (or relative) y position of the agent
        float yPos = this.transform.localPosition.y;
        // TODO: determine how to tell when the agent has fallen off the platform
        // and what to do when that happens
        



    }

    // For manual check of controls 
    public override void Heuristic(float[] actionsOut)
{
    actionsOut[0] = Input.GetAxis("Horizontal");
    actionsOut[1] = Input.GetAxis("Vertical");
}


}
