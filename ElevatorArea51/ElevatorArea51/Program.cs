using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace ElevatorArea51
{
    /// <summary>
    /// agents arrive at work
    /// they make decisions: to do missions, to go to another floor or to leave
    /// when they go to another floor they use the elevator
    /// the elevator has maximum seats allowing maximum threads to use it at a time
    /// more than one agent can enter 
    /// when the elevator is mooving, moving is locked until it reaches the destination floor
    /// other fields and methods of the elevator can still be used
    /// agents have different levels of security so their acces to floor might be denied
    /// in this case they choose another floor
    /// when an agent sets his/her event eventLeftWork, function HasLeft returns response
    /// when all agents return response that they left, show is over
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Elevator elevator = new Elevator();

            Array values = Enum.GetValues(typeof(AgentSecurityLevel));
            Random random = new Random();

            //var agentsList =
            //    Enumerable.Range(1, 6)
            //    .Select(i => new Agent()
            //    {
            //        Name = i.ToString(),
            //        Elevator = elevator,
            //        SecurityLevel = (AgentSecurityLevel)values.GetValue(random.Next(values.Length))
            //    }).ToList();

            Agent agent1 = new Agent("01_Confidential", AgentSecurityLevel.Confidential, elevator);
            Agent agent2 = new Agent("02_Secret", AgentSecurityLevel.Secret, elevator);
            Agent agent3 = new Agent("03_TopSecret", AgentSecurityLevel.TopSecret, elevator);
            //Agent agent4 = new Agent("04_Confidential", AgentSecurityLevel.Confidential, elevator);
            //Agent agent5 = new Agent("05_Secret", AgentSecurityLevel.Secret, elevator);
            //Agent agent6 = new Agent("06_TopSecret", AgentSecurityLevel.TopSecret, elevator);

            List<Agent> agentsList = new List<Agent>();
            agentsList.Add(agent1);
            agentsList.Add(agent2);
            agentsList.Add(agent3);
            //agentsList.Add(agent4);
            //agentsList.Add(agent5);
            //agentsList.Add(agent6);

            foreach (var agent in agentsList)
            {
                agent.SaveTheWorld();
            }

            while (agentsList.Any(a => ! a.HasLeft))
            {

            }
            Console.WriteLine("Show is over");
            Console.ReadLine();
        }
    }
}










//Array values = elevator.GetButtons();
//Random random = new Random();
//Decision randomDecision = (Decision)values.GetValue(random.Next(values.Length));
