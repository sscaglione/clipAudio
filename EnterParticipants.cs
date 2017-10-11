using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clip_Audio_CSL3
{
    class EnterParticipants
    {
        private System.Windows.Forms.ComboBox participantsDropdown = new System.Windows.Forms.ComboBox();
        private System.Windows.Forms.Label dropDownLabel = new System.Windows.Forms.Label();
        Button confirmation = new Button();
        private Form enterParticipants = new Form()
        {
            Width = 500,
            Height = 150,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterScreen
        };

        public EnterParticipants()
        {

        }

        public string ShowDialog(List<string> participantsEntriesList)
        {
            this.enterParticipants.SuspendLayout();


            this.confirmation.AutoSize = true;
            this.confirmation.Text = "Ok";
            this.confirmation.Location = new System.Drawing.Point(350, 75);
            this.confirmation.Enabled = false;
            //{ Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            this.confirmation.Click += (sender, e) => { enterParticipants.Close(); };

            participantsEntriesList.Insert(0, "");
            this.participantsDropdown.DataSource = participantsEntriesList;
            this.participantsDropdown.Location = new System.Drawing.Point(10, 40);
            this.participantsDropdown.Size = new System.Drawing.Size(300, 20);
            this.participantsDropdown.SelectedIndexChanged += new System.EventHandler(participantSelectChanged);
            //participantsDropdown.SelectedIndex = 0;

            this.dropDownLabel.AutoSize = true;
            this.dropDownLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropDownLabel.Location = new System.Drawing.Point(10, 10);
            this.dropDownLabel.Name = "dropDownLabel";
            this.dropDownLabel.Size = new System.Drawing.Size(295, 16);
            this.dropDownLabel.Text = "Select the team and participants to run.";

            this.enterParticipants.Controls.Add(confirmation);
            this.enterParticipants.Controls.Add(participantsDropdown);
            this.enterParticipants.Controls.Add(dropDownLabel);
            //enterParticipants.AcceptButton = confirmation;

            this.enterParticipants.PerformLayout();
            this.enterParticipants.ShowDialog();

            return this.participantsDropdown.SelectedValue.ToString();
        }

        private void participantSelectChanged(object sender, EventArgs e)
        {
            if (this.participantsDropdown.SelectedIndex > 0)
                this.confirmation.Enabled = true;
            else
                this.confirmation.Enabled = false;
        }

        public bool fileGenerated(string filename, string teamNum)
        {
            return System.IO.File.Exists(Constants.AUDIO_DIRECTORY_OUT + filename);
        }
    }
}
