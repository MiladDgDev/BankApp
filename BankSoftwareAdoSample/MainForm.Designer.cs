namespace BankSoftwareAdoSample
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            customerDataGridView = new DataGridView();
            customersLabel = new Label();
            addCustomerBtn = new Button();
            deleteCustomerBtn = new Button();
            updateCustomerBtn = new Button();
            fetchAllCustomersBtn = new Button();
            managemAccountsBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)customerDataGridView).BeginInit();
            SuspendLayout();
            // 
            // customerDataGridView
            // 
            customerDataGridView.AllowUserToAddRows = false;
            customerDataGridView.AllowUserToDeleteRows = false;
            customerDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            customerDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            customerDataGridView.ColumnHeadersHeight = 45;
            customerDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            customerDataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            customerDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            customerDataGridView.Location = new Point(13, 133);
            customerDataGridView.Margin = new Padding(4, 5, 4, 5);
            customerDataGridView.MultiSelect = false;
            customerDataGridView.Name = "customerDataGridView";
            customerDataGridView.ReadOnly = true;
            customerDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            customerDataGridView.Size = new Size(1032, 437);
            customerDataGridView.TabIndex = 0;
            // 
            // customersLabel
            // 
            customersLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            customersLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            customersLabel.Location = new Point(13, 9);
            customersLabel.Margin = new Padding(4, 0, 4, 0);
            customersLabel.Name = "customersLabel";
            customersLabel.Size = new Size(1032, 51);
            customersLabel.TabIndex = 1;
            customersLabel.Text = "Customers";
            customersLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // addCustomerBtn
            // 
            addCustomerBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            addCustomerBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addCustomerBtn.Location = new Point(523, 72);
            addCustomerBtn.Name = "addCustomerBtn";
            addCustomerBtn.Size = new Size(126, 45);
            addCustomerBtn.TabIndex = 2;
            addCustomerBtn.Text = "Add";
            addCustomerBtn.UseVisualStyleBackColor = true;
            addCustomerBtn.Click += addCustomerBtn_Click;
            // 
            // deleteCustomerBtn
            // 
            deleteCustomerBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            deleteCustomerBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            deleteCustomerBtn.Location = new Point(787, 72);
            deleteCustomerBtn.Name = "deleteCustomerBtn";
            deleteCustomerBtn.Size = new Size(126, 45);
            deleteCustomerBtn.TabIndex = 3;
            deleteCustomerBtn.Text = "Delete";
            deleteCustomerBtn.UseVisualStyleBackColor = true;
            deleteCustomerBtn.Click += deleteCustomerBtn_Click;
            // 
            // updateCustomerBtn
            // 
            updateCustomerBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            updateCustomerBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            updateCustomerBtn.Location = new Point(655, 72);
            updateCustomerBtn.Name = "updateCustomerBtn";
            updateCustomerBtn.Size = new Size(126, 45);
            updateCustomerBtn.TabIndex = 4;
            updateCustomerBtn.Text = "Update";
            updateCustomerBtn.UseVisualStyleBackColor = true;
            updateCustomerBtn.Click += UpdateCustomerBtn_Click;
            // 
            // fetchAllCustomersBtn
            // 
            fetchAllCustomersBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            fetchAllCustomersBtn.Location = new Point(13, 72);
            fetchAllCustomersBtn.Name = "fetchAllCustomersBtn";
            fetchAllCustomersBtn.Size = new Size(144, 45);
            fetchAllCustomersBtn.TabIndex = 5;
            fetchAllCustomersBtn.Text = "Fetch All";
            fetchAllCustomersBtn.UseVisualStyleBackColor = true;
            fetchAllCustomersBtn.Click += fetchAllCustomersBtn_Click;
            // 
            // managemAccountsBtn
            // 
            managemAccountsBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            managemAccountsBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            managemAccountsBtn.Location = new Point(919, 72);
            managemAccountsBtn.Name = "managemAccountsBtn";
            managemAccountsBtn.Size = new Size(126, 45);
            managemAccountsBtn.TabIndex = 6;
            managemAccountsBtn.Text = "Accounts";
            managemAccountsBtn.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(15F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1058, 584);
            Controls.Add(managemAccountsBtn);
            Controls.Add(fetchAllCustomersBtn);
            Controls.Add(updateCustomerBtn);
            Controls.Add(deleteCustomerBtn);
            Controls.Add(addCustomerBtn);
            Controls.Add(customersLabel);
            Controls.Add(customerDataGridView);
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 5, 4, 5);
            MaximumSize = new Size(1920, 1080);
            MinimumSize = new Size(1080, 640);
            Name = "MainForm";
            Text = "Dashboard";
            FormClosing += MainForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)customerDataGridView).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView customerDataGridView;
        private System.Windows.Forms.Label customersLabel;

        #endregion

        private Button addCustomerBtn;
        private Button deleteCustomerBtn;
        private Button updateCustomerBtn;
        private Button fetchAllCustomersBtn;
        private Button managemAccountsBtn;
    }
}
