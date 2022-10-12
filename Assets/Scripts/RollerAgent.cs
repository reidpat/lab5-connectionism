using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


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

        ResetTargetPosition();
    }

    public void ResetTargetPosition(){
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
    public override void OnActionReceived(ActionBuffers actions){

        
        // The actions are the output from the neural network
        // These actions are converted into a force which push the ball in some direction along the 2D plane (x and z axis)
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);
        


        // the local (or relative) y position of the agent
        float yPos = this.transform.localPosition.y;
        // TODO: determine how to tell when the agent has fallen off the platform
        // and what to do when that happens
        



    }

    //Detect collisions between the GameObjects with Colliders attached
void OnTriggerEnter(Collider other){

         // TODO: determine when to give reward to the agent, the amount of reward,
        // and what happens after reward is given
        // Hint: check the Agents documentation for relevant functions to use
        if (other.name == "Target")
        {
            
        }
    }

    // For manual check of controls 
    public override void Heuristic(in ActionBuffers actionsOut)
{
     ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
    continuousActions[0] = Input.GetAxis("Horizontal");
    continuousActions[1] = Input.GetAxis("Vertical");
}


}
