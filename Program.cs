using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Clip_Audio_CSL3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                List<string> participantsEntriesList = new List<string>();
                CSVHandler.CSVHandler csvParticipants = new CSVHandler.CSVHandler(Constants.PARTICIPANTS_LIST);
                while (!csvParticipants.endOfData())
                    participantsEntriesList.Add(csvParticipants.getNextRow()["Participants_Entry"]);

                EnterParticipants entries = new EnterParticipants();
                string[] participants = null;

                bool fileGenerated = true;
                while (fileGenerated)
                {
                    participants = entries.ShowDialog(participantsEntriesList).Split(' ');
                        fileGenerated = entries.fileGenerated(participants[1] + "-audio-clipped.wav", participants[0]);
                    if (fileGenerated)
                    {
                        MessageBox.Show("This participant already has a clipped video. Please select another.");
                    }

                }

                string teamNum = participants[0];
                string subjectIDA = participants[1];
                string subjectIDB = participants[2];
                string subjectIDC = participants[3];
                string participant = null;

                for (int i = 0; i < 4; i++)
                {

                    switch (Constants.PARTICIPANT[i])
                    {
                        case 0:
                            participant = subjectIDA;
                            break;
                        case 1:
                            participant = subjectIDB;
                            break;
                        case 2:
                            participant = subjectIDC;
                            break;
                        case 3:
                            participant = "Zoom";
                            break;
                    }

                    ClipAudio clipParticipant;

                    if (Constants.PARTICIPANT[i] == 3)
                    {
                        clipParticipant = new ClipAudio(
                            Constants.INTERACTION_DIRECTORY + teamNum + "\\CSL3-" + teamNum + "-interaction.txt",
                            Constants.RATING_POSITIONS_DIRECTORY + teamNum + "\\CSL3-" + teamNum + "-master-rating-positions.txt",
                            Constants.CUT_LOG_DIRECTORY + teamNum + "\\" + teamNum + "-" + participant + "-cut-log.txt",
                            teamNum, subjectIDA, subjectIDB, subjectIDC, participant);
                        clipParticipant.getZoomStartStopTime();
                        clipParticipant.performClip(Constants.PARTICIPANT[i]);
                    }

                    else
                    {
                        clipParticipant = new ClipAudio(
                            Constants.FRAMELOG_DIRECTORY + "\\" + teamNum + "\\" + participant + "-Challenges-framelog.txt",
                            Constants.RATING_POSITIONS_DIRECTORY + teamNum + "\\CSL3-" + teamNum + "-master-rating-positions.txt",
                            Constants.CUT_LOG_DIRECTORY + participant + "-cut-log.txt",
                            teamNum, subjectIDA, subjectIDB, subjectIDC, participant);
                        clipParticipant.getStartStopTime();
                        clipParticipant.performClip(Constants.PARTICIPANT[i]);
                        string sourceFile = Constants.CUT_LOG_DIRECTORY + participant + "-cut-log.txt";
                        string destinationFile = Constants.CUT_LOG_DIRECTORY + teamNum + "\\" + participant + "-cut-log.txt";
                        // To move a file or folder to a new location:
                        System.IO.File.Move(sourceFile, destinationFile);
                    }


                }

        }
    }
}