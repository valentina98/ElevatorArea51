using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace ElevatorArea51
{
    enum AgentSecurityLevel { Confidential, Secret, TopSecret };
    enum AgentWorkDecision { MoveAround, GoToRandomFloor, Leave };

    class Agent
    {
        public AgentSecurityLevel SecurityLevel { get; set; }
        public int CurrentFloor;

        public string Name { get; set; }
        public Elevator Elevator { get; set; }
        Random random = new Random();
        ManualResetEvent eventLeftWork = new ManualResetEvent(false);

        /* this constructor should only be used for testing */
        /* in order to work you should manually set "string n, AgentSecurityLevel sl, Elevator e" */
        public Agent()
        {

        }

        public Agent(string n, AgentSecurityLevel sl, Elevator e)
        {
            this.Name = n;
            this.SecurityLevel = sl;
            this.Elevator = e;
            this.CurrentFloor = 0;
        }

        /* go to specified floor and if door does not open choose another floor */
        /* mind that everyone should have access to floor 0 in order to exit because this method is used */
        private void UseElevator(int floor)
        {
            // call the elevator
            Console.WriteLine(Name + " waits in line for the elevator");
            Elevator.MoveToFloor(CurrentFloor); // that returns true. Is it a bad practise?

            Console.WriteLine(Name + " enters the elevator");
            Elevator.Enter(this);
            Console.WriteLine(Name + " wants to go from floor " + CurrentFloor + " to floor " + floor.ToString() );

            // if you don't do that and the confidential agent enters the elevator, none of them will ever exit
            CurrentFloor = -1; // to be able to get another floor including the floor that the agent entered at
            
            // Elevator goes to specified floor, if the door does not open door, the agent choses another floor
            while (!Elevator.MoveToFloor(floor))
            {
                // writeline before and after new decision
                Console.WriteLine(Name + " couldn't go to floor " + floor.ToString());
                floor = GetAnotherFloor();
                Console.WriteLine(Name + " desided to go to floor " + floor.ToString());
            }
            Elevator.Exit(this);
            Console.WriteLine(Name + " exited the elevator at floor " + floor);
            CurrentFloor = floor;
        }
        /* simulates an agent workind until he leaves */
        private void Work()
        {
            Console.WriteLine(Name + " arrived at work" );
            AgentWorkDecision decision;
            do
            {
                decision = GetRandomWorkDecision();
                switch (decision)
                {
                    case AgentWorkDecision.MoveAround:
                        // wait for 3 seconds for agent to move around
                        Console.WriteLine(Name + " does some saving missions");
                        Thread.Sleep(3000);
                        break;
                    case AgentWorkDecision.GoToRandomFloor:
                        // if access is not provided, new random floor will be chosen
                        Console.WriteLine(Name + " wants to go to another floor");
                        // get random floor which is not the current floor
                        UseElevator(GetAnotherFloor());
                        break;
                    case AgentWorkDecision.Leave:
                        //go to ground floor and leave work
                        Console.WriteLine(Name + " desides to leave");
                        if(CurrentFloor != 0) UseElevator(0);
                        Console.WriteLine(Name + " left");
                        eventLeftWork.Set();
                        break;
                    default:
                        throw new ArgumentException("This decision is not valid", "decision");
                }

            } while (decision != AgentWorkDecision.Leave);
        }
        // get random floor which is not the current one
        private int GetAnotherFloor()
        {
            int floor;
            do
            {
                floor = random.Next(Elevator.Floors.Count);
            } while (floor == CurrentFloor);
            return floor;
        }
        /* returns a random value from the enum AgentWorkDecision */
        private AgentWorkDecision GetRandomWorkDecision()
        {
            Array values = Enum.GetValues(typeof(AgentWorkDecision));
            //Random random = new Random();
            AgentWorkDecision randomWorkDecision = (AgentWorkDecision)values.GetValue(random.Next(values.Length));

            return randomWorkDecision;
        }
        /* start a thread which executes methodd Work() */
        public void SaveTheWorld()
        {
            Thread t = new Thread(Work);
            t.Start();
        }
        /* the method will not return response untill eventLeftWork is set for this agent */
        public bool HasLeft
        {
            get
            {
                return eventLeftWork.WaitOne(0);
            }
        }
    }
}







//public Agent()
//{
//    AgentsList.Add(this);
//}





            //AgentsList.Add(this);
        //}
        //public List<Agent> GetAgentList()
        //{
        //    return AgentsList;
        //}
