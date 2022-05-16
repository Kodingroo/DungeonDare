// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class PassTurnCommand : Command
// {

//     public PassTurnCommand()
//     {

//     }

//     public override void StartCommandExecution()
//     {
//         foreach (KeyValuePair<int, EnemyLogic> el in EnemyLogic.EnemiesCreatedThisGame)
//         {
//             Debug.Log("PassTurnCommand EnemyLogic ID: "+el.Value.ID);
//             el.Value.GoFace();
//         }
//     }
// }
