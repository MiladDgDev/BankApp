namespace BankSoftwareAdoSample
{
    partial class UpdateCustomerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            firstNameLabel = new Label();
            lastNameLabel = new Label();
            streetLabel = new Label();
            firstNameTextBox = new TextBox();
            addCustomerTitle = new Label();
            lastNameTextBox = new TextBox();
            streetTextBox = new TextBox();
            cityTextBox = new TextBox();
            cityLabel = new Label();
            houseNoLabel = new Label();
            houseNoTextBox = new TextBox();
            zipCodeLabel = new Label();
            zipCodeTextBox = new TextBox();
            EmailLabel = new Label();
            emailTextBox = new TextBox();
            phoneNoLabel = new Label();
            phoneNoTextBox = new TextBox();
            updateCustomerBtn = new Button();
            SuspendLayout();
            // 
            // firstNameLabel
            // 
            firstNameLabel.Location = new Point(37, 147);
            firstNameLabel.Margin = new Padding(4, 0, 4, 0);
            firstNameLabel.Name = "firstNameLabel";
            firstNameLabel.Size = new Size(168, 45);
            firstNameLabel.TabIndex = 0;
            firstNameLabel.Text = "First Name";
            firstNameLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lastNameLabel
            // 
            lastNameLabel.Location = new Point(536, 147);
            lastNameLabel.Margin = new Padding(4, 0, 4, 0);
            lastNameLabel.Name = "lastNameLabel";
            lastNameLabel.Size = new Size(168, 45);
            lastNameLabel.TabIndex = 1;
            lastNameLabel.Text = "Last Name";
            lastNameLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // streetLabel
            // 
            streetLabel.Location = new Point(37, 223);
            streetLabel.Margin = new Padding(4, 0, 4, 0);
            streetLabel.Name = "streetLabel";
            streetLabel.Size = new Size(168, 45);
            streetLabel.TabIndex = 2;
            streetLabel.Text = "Street";
            streetLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // firstNameTextBox
            // 
            firstNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            firstNameTextBox.Location = new Point(213, 147);
            firstNameTextBox.Margin = new Padding(4, 5, 4, 5);
            firstNameTextBox.Name = "firstNameTextBox";
            firstNameTextBox.Size = new Size(267, 45);
            firstNameTextBox.TabIndex = 1;
            // 
            // addCustomerTitle
            // 
            addCustomerTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            addCustomerTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addCustomerTitle.Location = new Point(346, 9);
            addCustomerTitle.Margin = new Padding(4, 0, 4, 0);
            addCustomerTitle.Name = "addCustomerTitle";
            addCustomerTitle.Size = new Size(408, 85);
            addCustomerTitle.TabIndex = 4;
            addCustomerTitle.Text = "Update Customer";
            addCustomerTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lastNameTextBox
            // 
            lastNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lastNameTextBox.Location = new Point(712, 147);
            lastNameTextBox.Margin = new Padding(4, 5, 4, 5);
            lastNameTextBox.Name = "lastNameTextBox";
            lastNameTextBox.Size = new Size(267, 45);
            lastNameTextBox.TabIndex = 2;
            // 
            // streetTextBox
            // 
            streetTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            streetTextBox.Location = new Point(213, 223);
            streetTextBox.Margin = new Padding(4, 5, 4, 5);
            streetTextBox.Name = "streetTextBox";
            streetTextBox.Size = new Size(267, 45);
            streetTextBox.TabIndex = 3;
            // 
            // cityTextBox
            // 
            cityTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cityTextBox.Location = new Point(213, 302);
            cityTextBox.Margin = new Padding(4, 5, 4, 5);
            cityTextBox.Name = "cityTextBox";
            cityTextBox.Size = new Size(267, 45);
            cityTextBox.TabIndex = 5;
            // 
            // cityLabel
            // 
            cityLabel.Location = new Point(141, 302);
            cityLabel.Margin = new Padding(4, 0, 4, 0);
            cityLabel.Name = "cityLabel";
            cityLabel.Size = new Size(64, 45);
            cityLabel.TabIndex = 10;
            cityLabel.Text = "City";
            cityLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // houseNoLabel
            // 
            houseNoLabel.Location = new Point(536, 223);
            houseNoLabel.Margin = new Padding(4, 0, 4, 0);
            houseNoLabel.Name = "houseNoLabel";
            houseNoLabel.Size = new Size(168, 45);
            houseNoLabel.TabIndex = 12;
            houseNoLabel.Text = "House No.";
            houseNoLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // houseNoTextBox
            // 
            houseNoTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            houseNoTextBox.Location = new Point(712, 223);
            houseNoTextBox.Margin = new Padding(4, 5, 4, 5);
            houseNoTextBox.Name = "houseNoTextBox";
            houseNoTextBox.Size = new Size(267, 45);
            houseNoTextBox.TabIndex = 4;
            // 
            // zipCodeLabel
            // 
            zipCodeLabel.Location = new Point(555, 302);
            zipCodeLabel.Margin = new Padding(4, 0, 4, 0);
            zipCodeLabel.Name = "zipCodeLabel";
            zipCodeLabel.Size = new Size(149, 45);
            zipCodeLabel.TabIndex = 14;
            zipCodeLabel.Text = "Zip Code";
            zipCodeLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // zipCodeTextBox
            // 
            zipCodeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            zipCodeTextBox.Location = new Point(712, 302);
            zipCodeTextBox.Margin = new Padding(4, 5, 4, 5);
            zipCodeTextBox.Name = "zipCodeTextBox";
            zipCodeTextBox.Size = new Size(267, 45);
            zipCodeTextBox.TabIndex = 6;
            // 
            // EmailLabel
            // 
            EmailLabel.Location = new Point(614, 378);
            EmailLabel.Margin = new Padding(4, 0, 4, 0);
            EmailLabel.Name = "EmailLabel";
            EmailLabel.Size = new Size(90, 45);
            EmailLabel.TabIndex = 18;
            EmailLabel.Text = "Email";
            EmailLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // emailTextBox
            // 
            emailTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            emailTextBox.Location = new Point(712, 378);
            emailTextBox.Margin = new Padding(4, 5, 4, 5);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new Size(267, 45);
            emailTextBox.TabIndex = 8;
            // 
            // phoneNoLabel
            // 
            phoneNoLabel.Location = new Point(104, 378);
            phoneNoLabel.Margin = new Padding(4, 0, 4, 0);
            phoneNoLabel.Name = "phoneNoLabel";
            phoneNoLabel.Size = new Size(101, 45);
            phoneNoLabel.TabIndex = 16;
            phoneNoLabel.Text = "Phone";
            phoneNoLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // phoneNoTextBox
            // 
            phoneNoTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            phoneNoTextBox.Location = new Point(213, 378);
            phoneNoTextBox.Margin = new Padding(4, 5, 4, 5);
            phoneNoTextBox.Name = "phoneNoTextBox";
            phoneNoTextBox.Size = new Size(267, 45);
            phoneNoTextBox.TabIndex = 7;
            // 
            // updateCustomerBtn
            // 
            updateCustomerBtn.BackColor = SystemColors.ButtonHighlight;
            updateCustomerBtn.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            updateCustomerBtn.ForeColor = SystemColors.HotTrack;
            updateCustomerBtn.Location = new Point(432, 482);
            updateCustomerBtn.Name = "updateCustomerBtn";
            updateCustomerBtn.Size = new Size(236, 90);
            updateCustomerBtn.TabIndex = 9;
            updateCustomerBtn.Text = "UPDATE";
            updateCustomerBtn.UseVisualStyleBackColor = false;
            updateCustomerBtn.UseWaitCursor = true;
            updateCustomerBtn.Click += updateCustomerBtn_Click;
            // 
            // UpdateCustomerForm
            // 
            AutoScaleDimensions = new SizeF(15F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1058, 584);
            Controls.Add(updateCustomerBtn);
            Controls.Add(EmailLabel);
            Controls.Add(emailTextBox);
            Controls.Add(phoneNoLabel);
            Controls.Add(phoneNoTextBox);
            Controls.Add(zipCodeLabel);
            Controls.Add(zipCodeTextBox);
            Controls.Add(houseNoLabel);
            Controls.Add(houseNoTextBox);
            Controls.Add(cityLabel);
            Controls.Add(cityTextBox);
            Controls.Add(streetTextBox);
            Controls.Add(lastNameTextBox);
            Controls.Add(addCustomerTitle);
            Controls.Add(firstNameTextBox);
            Controls.Add(streetLabel);
            Controls.Add(lastNameLabel);
            Controls.Add(firstNameLabel);
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 5, 4, 5);
            MaximumSize = new Size(1080, 640);
            MinimumSize = new Size(1080, 640);
            Name = "UpdateCustomerForm";
            Text = "Update Customer";
            Load += UpdateCustomerForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label firstNameLabel;
        private Label lastNameLabel;
        private Label streetLabel;
        private TextBox firstNameTextBox;
        private Label addCustomerTitle;
        private TextBox lastNameTextBox;
        private TextBox streetTextBox;
        private TextBox cityTextBox;
        private Label cityLabel;
        private Label houseNoLabel;
        private TextBox houseNoTextBox;
        private Label zipCodeLabel;
        private TextBox zipCodeTextBox;
        private Label EmailLabel;
        private TextBox emailTextBox;
        private Label phoneNoLabel;
        private TextBox phoneNoTextBox;
        private Button updateCustomerBtn;
    }

}