using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clip_Audio_CSL3
{
    class Constants
    {
        //Participant B
        public static readonly string PARTICIPANTS_LIST = "C:\\EmotiveComputingLab\\Dropbox (Emotive Computing)\\Studies\\CSL3\\participants.txt";
        public static readonly string RATING_POSITIONS_DIRECTORY = "C:\\EmotiveComputingLab\\Dropbox (Emotive Computing)\\Studies\\CSL3\\Output\\";
        public static readonly string AUDIO_DIRECTORY_IN = "C:\\EmotiveComputingLab\\Dropbox (Emotive Computing)\\Studies\\CSL3\\Output\\"; //only used if Zoom
        public static readonly string AUDIO_DIRECTORY_OUT = "C:\\Users\\emotive.computing\\Desktop\\CSL3\\Clip-Audio-Output\\";
        public static readonly string FRAMELOG_DIRECTORY = "C:\\EmotiveComputingLab\\Dropbox (Emotive Computing)\\Studies\\CSL3\\Output";
        public static readonly string INTERACTION_DIRECTORY = "C:\\EmotiveComputingLab\\Dropbox (Emotive Computing)\\Studies\\CSL3\\Output\\";
        public static readonly string CUT_LOG_DIRECTORY = "C:\\Users\\emotive.computing\\Desktop\\CSL3\\Clip-Audio-Output\\";
        public static readonly string FFMPEG_PATH = "C:\\Users\\emotive.computing\\Desktop\\CSL3\\FFmpeg\\ffmpeg.exe";
        public static readonly int [] PARTICIPANT = new int[4] {0,1,2,3}; //0 for A, 1 for B, 2 for C, 3 for Zoom

        /*public static readonly string PARTICIPANTS_LIST = "C:\\Users\\astewa12\\Dropbox (Emotive Computing)\\Source\\AffectVideoJudgement-CSL3\\AffectVideoJudgement-CSL3\\participants.txt";
        public static readonly string RATING_POSITIONS_DIRECTORY = "C:\\Users\\astewa12\\Dropbox (Emotive Computing)\\Data\\CSL3\\TEST\\";
        public static readonly string VIDEOS_DIRECTORY_IN = "C:\\Users\\astewa12\\Dropbox (Emotive Computing)\\Data\\CSL3\\TEST\\";
        public static readonly string VIDEOS_DIRECTORY_OUT = "C:\\Users\\astewa12\\Dropbox (Emotive Computing)\\Data\\CSL3\\TEST\\";
        public static readonly string FRAMELOG_DIRECTORY = "C:\\Users\\astewa12\\Dropbox (Emotive Computing)\\Data\\CSL3\\TEST\\";
        public static readonly string INTERACTION_DIRECTORY = "C:\\Users\\astewa12\\Dropbox (Emotive Computing)\\Data\\CSL3\\TEST\\";
        public static readonly string CUT_LOG_DIRECTORY = "C:\\Users\\astewa12\\Dropbox (Emotive Computing)\\Data\\CSL3\\TEST\\";
        public static readonly string FFMPEG_PATH = "C:\\Users\\astewa12\\Dropbox (Emotive Computing)\\Source\\FFmpeg\\ffmpeg.exe";
        public static readonly int PARTICIPANT = 3; //0 for A, 1 for B, 2 for C, 3 for Zoom*/
        

    }
}
