using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace ElevatorArea51
{
    //enum SecurityLevel { G, S, T1, T2 };
    class Elevator
    {
        private int Speed;
        public int CurrentFloor;
        public Dictionary<int, string> Floors; // agent uses it to get random floor
        private readonly object locker = new object(); // an object used to lock the elevator while moving

        Semaphore semaphore;
        public List<Agent> agents; // list of agents in the elevator

        /* speed is default 1sec per floor and semaphore has default 4 concurrent threads*/
        public Elevator(int s = 1, int maxSeats = 4)
        {
            Speed = s;
            CurrentFloor = 0;

            Floors = new Dictionary<int, string>();
            Floors.Add(0, "G");
            Floors.Add(1, "S");
            Floors.Add(2, "T1");
            Floors.Add(3, "T2");

            semaphore = new Semaphore(maxSeats, maxSeats);
            agents = new List<Agent>();
        }
        /* add agent to list and to the semaphore */
        public void Enter(Agent agent)
        {
            semaphore.WaitOne();
            lock (agents)
            {
                agents.Add(agent);
            }
        }
        /* remove agent from agent list and relese semaphore */
        public void Exit(Agent agent)
        {
            semaphore.Release();
            lock (agents)
            {
                agents.Remove(agent);
            }
        }
        /* wait; assign new value to CurrentFloor; try to open the door; return true if door opens */
        public bool MoveToFloor(int toFloor)
        {
            // else make elevator unusable until it reaches a floor
            lock(locker)
               {
                // if the elevator is on the target floor return if it the agents are able to exit / enter 
                if (CurrentFloor == toFloor)
                {
                    return CanOpenDoor();
                }


                Console.WriteLine("Elevator moving from floor " + CurrentFloor.ToString() + " to floor " + toFloor.ToString());

                // wait for elevator to reach floor ( 1sec per floor if speed is 1 )
                for (int i = 0; i < Math.Abs(toFloor - CurrentFloor); i++)
                {
                    Thread.Sleep(1000 * Speed);
                }
                CurrentFloor = toFloor;

                Console.WriteLine("Elevator reached floor " + CurrentFloor.ToString());

            }

            // return false if agents cannot exit
            // comment this line to skip security
            if (!CanOpenDoor()) return false;

            return true;
        }
        /* check if agents in the elevator have the needed security level */
        private bool CanOpenDoor()
        {
            return CurrentFloor <= GetSecurityLevel();
        }
        /* returns the minimum acces level of the agents in agent list */
        private int GetSecurityLevel()
        {
            int accessLevel = 3; // the maximum access level
            foreach( Agent a in agents )
            {
                switch (a.SecurityLevel)
                {
                    case AgentSecurityLevel.Confidential: 
                        if(0 < accessLevel) accessLevel = 0;
                        break;
                    case AgentSecurityLevel.Secret:
                        if (1 < accessLevel) accessLevel = 1;
                        break;
                    case AgentSecurityLevel.TopSecret:
                        // he can access all levels
                        break;
                    default:
                        throw new ArgumentException("This security level is not valid", "a.SecurityLevel");
                }
            }
            return accessLevel;
        }
    }
}










//public ElevatorButton[] GetButtons()
//{
//    List<ElevatorButton> buttonsList = new List<ElevatorButton>();
//    foreach (ElevatorButton button in Enum.GetValues(typeof(ElevatorButton)))
//    {
//        buttonsList.Add(button);
//    }
//    return buttonsList.ToArray();
//}

