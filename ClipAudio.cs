using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Clip_Audio_CSL3
{
    class ClipAudio
    {
        private string masterFile;
        private string cutLog;
        private string frameLog;
        private string teamNum;
        private string subjectIDA;
        private string subjectIDB;
        private string subjectIDC;
        private string participant;
        private double startTimeMinDiff;
        private double stopTimeMinDiff;
        private DateTime startTimeStamp;
        private DateTime stopTimeStamp;
        private DateTime audStartTime;
        private TimeSpan startTimeIntoAud;
        private TimeSpan stopTimeIntoAud;

        public ClipAudio(string frameLog, string masterRatingPositions, string cutLog, string teamNum, string subjectIDA, string subjectIDB, string subjectIDC, string participant)
        {
            //bool fileOpened = false;
            this.teamNum = teamNum;
            this.subjectIDA = subjectIDA;
            this.subjectIDB = subjectIDB;
            this.subjectIDC = subjectIDC;
            this.participant = participant;
            this.frameLog = frameLog;
            this.masterFile = masterRatingPositions;
            this.cutLog = cutLog;

        }

        //only use this one for participant files, not Zoom files
        //gets the start and stop time of the Challenges from master-rating-positions
        //finds time into audio to start and stop
        public void getStartStopTime()
        {
            Logger.Logger log = new Logger.Logger(this.cutLog, "TeamNumber", "subjectID_A", "subjectID_B", "subjectID_C", "MinDiffStart", "MinDiffStartTimeStamp", "StartTimeIntoAud", "MinDiffStop", "MinDiffStopTimeStamp", "StopTimeIntoAud");
            
            StreamReader masterRatingReader = new StreamReader(this.masterFile);
            masterRatingReader.ReadLine();
            DateTime masterStartTime = Convert.ToDateTime(masterRatingReader.ReadLine().Split('\t')[4]);
            DateTime masterEndTime = DateTime.MinValue;

            string currentLine;
            while ((currentLine = masterRatingReader.ReadLine()) != null)
            {
                masterEndTime = Convert.ToDateTime(currentLine.Split('\t')[4]);
            }

            masterRatingReader.Close();

            StreamReader frameLogReader = new StreamReader(this.frameLog);
            bool header = true;
            bool first = true;
            double minDiffStart = Double.PositiveInfinity;
            DateTime minDiffStartTimeStamp = DateTime.MinValue;
            double minDiffStop = Double.PositiveInfinity;
            DateTime minDiffStopTimeStamp = DateTime.MinValue;

            while ((currentLine = frameLogReader.ReadLine()) != null)
            {
                if (header)
                {
                    header = false;
                    continue;
                }


                DateTime frameLogTimeStamp = Convert.ToDateTime(currentLine.Split('\t')[2]);

                if (first)
                {
                    first = false;
                    this.audStartTime = frameLogTimeStamp;
                }

                double currentDiff = Math.Abs(masterStartTime.Subtract(frameLogTimeStamp).TotalSeconds);
                if (currentDiff < minDiffStart)
                {
                    minDiffStart = currentDiff;
                    minDiffStartTimeStamp = frameLogTimeStamp;
                }

                currentDiff = Math.Abs(masterEndTime.Subtract(frameLogTimeStamp).TotalSeconds);
                if (currentDiff < minDiffStop)
                {
                    minDiffStop = currentDiff;
                    minDiffStopTimeStamp = frameLogTimeStamp;
                }
                
            }

            this.startTimeMinDiff = minDiffStart;
            this.startTimeStamp = minDiffStartTimeStamp;
            this.stopTimeMinDiff = minDiffStop;
            this.stopTimeStamp = minDiffStopTimeStamp;
            

            frameLogReader.Close();

            this.startTimeIntoAud = this.startTimeStamp.Subtract(this.audStartTime);
            this.stopTimeIntoAud = this.stopTimeStamp.Subtract(this.audStartTime);
            log.log(this.teamNum, this.subjectIDA, this.subjectIDB, this.subjectIDC, Convert.ToString(this.startTimeMinDiff), this.startTimeStamp.ToString("MM/dd/yyyy hh:mm:ss.fff"), this.startTimeIntoAud.ToString(@"hh\:mm\:ss\.fff"), Convert.ToString(this.stopTimeMinDiff), this.stopTimeStamp.ToString("MM/dd/yyyy hh:mm:ss.fff"), this.stopTimeIntoAud.ToString(@"hh\:mm\:ss\.fff"));
            log.close();
        }

        public void performClip(int participant)
        {
            string inAudio;
            string outAudio;
            string arguments = "";
                if (participant == 3)
                {
                    inAudio = " \"" + Constants.AUDIO_DIRECTORY_IN + this.teamNum + "\\" + this.teamNum + "-audio_only_1.m4a\"";
                    outAudio = " \"" + Constants.AUDIO_DIRECTORY_OUT + this.teamNum + "\\" + this.teamNum + "-audio_only_1-clipped.wav\"";
                    arguments = "-i " + inAudio + " -ss " + this.startTimeIntoAud.ToString(@"hh\:mm\:ss\.fff") + " -to " + this.stopTimeIntoAud.ToString(@"hh\:mm\:ss\.fff") + " " + outAudio;
                    Console.WriteLine(arguments);
                    Process.Start(Constants.FFMPEG_PATH, arguments);
                }

                else
                {
                    inAudio = " \"" + Constants.AUDIO_DIRECTORY_IN + this.teamNum + "\\" + this.participant + "-Challenges-audio.wav\""; //added this.teamNum
                    // Try to create the directory in case it doesn't already exist.
                    DirectoryInfo di = Directory.CreateDirectory("C:\\Users\\emotive.computing\\Desktop\\CSL3\\Clip-Audio-Output\\" + this.teamNum);
                    outAudio = " \"" + Constants.AUDIO_DIRECTORY_OUT + this.teamNum + "\\" + this.participant + "-Challenges-audio-clipped.wav\"";
                    arguments = "-i " + inAudio + " -ss " + this.startTimeIntoAud.ToString(@"hh\:mm\:ss\.fff") + " -to " + this.stopTimeIntoAud.ToString(@"hh\:mm\:ss\.fff") + " " + outAudio;
                    Console.WriteLine(arguments);
                    Process.Start(Constants.FFMPEG_PATH, arguments);
                }
        }

        //only use this one for Zoom files
        public void getZoomStartStopTime()
        {
            Logger.Logger log = new Logger.Logger(this.cutLog, "TeamNumber", "subjectID_A", "subjectID_B", "subjectID_C", "StartTimeIntoAud", "StopTimeIntoAud");
            StreamReader masterRatingReader = new StreamReader(this.masterFile);
            masterRatingReader.ReadLine();
            DateTime masterStartTime = Convert.ToDateTime(masterRatingReader.ReadLine().Split('\t')[4]);
            DateTime masterEndTime = DateTime.MinValue;

            string currentLine;
            while ((currentLine = masterRatingReader.ReadLine()) != null)
            {
                masterEndTime = Convert.ToDateTime(currentLine.Split('\t')[4]);
            }

            masterRatingReader.Close();

            StreamReader interactionReader = new StreamReader(this.frameLog);
            bool secondStart = false;
            while ((currentLine = interactionReader.ReadLine()) != null)
            {
                string[] splitLine = currentLine.Split('\t');

                if (splitLine[6] == "StartZoomRecord") {
                    if (secondStart)
                    {
                        this.audStartTime = Convert.ToDateTime(splitLine[5]);
                        break;
                    }
                    secondStart = true;   
                }         
            }

            this.startTimeIntoAud = masterStartTime.Subtract(this.audStartTime);
            this.stopTimeIntoAud = masterEndTime.Subtract(this.audStartTime);

            log.log(this.teamNum, this.subjectIDA, this.subjectIDB, this.subjectIDC, this.startTimeIntoAud.ToString(@"hh\:mm\:ss\.fff"), this.stopTimeIntoAud.ToString(@"hh\:mm\:ss\.fff"));
            log.close();
        }
    }
}
