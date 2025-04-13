// Copyright (c) 2025 Akshat Khare
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using FieldSurveyMAUIApp.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp
{
    public partial class App : Application
    {
        private readonly IAuthService _authService;

        public App(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            MainPage = new AppShell();
        }
    }
}