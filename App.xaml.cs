﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO; // Add this line for file operations
using System.Text.Json; // For JSON serialization
using static NZTS_App.ChangelogUserControl;

namespace NZTS_App
{
    public partial class App : Application
    {
        public static ChangelogUserControl? changelogUserControl;
        public static ObservableCollection<ChangelogEntry> ChangelogEntries { get; } = new ObservableCollection<ChangelogEntry>();
        private const string LogFilePath = "changelog.json";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoadLogs();

            // Initialize the changelog user control
            changelogUserControl = new ChangelogUserControl
            {
                ChangelogEntries = ChangelogEntries // Set the entries
            };

            // Create and show the main window
            var mainWindow = new MainWindow
            {
                Content = changelogUserControl // Set the content of the main window
            };

           
        }
        public static void SaveLogs()
        {
            try
            {
                var json = JsonSerializer.Serialize(ChangelogEntries, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(LogFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving logs: {ex.Message}");
            }
        }

        public static void LoadLogs()
        {
            if (File.Exists(LogFilePath))
            {
                try
                {
                    var json = File.ReadAllText(LogFilePath);
                    var logs = JsonSerializer.Deserialize<ObservableCollection<ChangelogEntry>>(json);
                    if (logs != null)
                    {
                        // Clear existing entries before adding loaded ones
                        ChangelogEntries.Clear();
                        foreach (var log in logs)
                        {
                            ChangelogEntries.Add(log);
                        }
                    }
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error deserializing logs: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading logs: {ex.Message}");
                }
            }
        }
    }
}


