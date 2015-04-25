﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using OgreMaze.Core;
using OgreMaze.Core.Services;
using OgreMaze.UI.Annotations;

namespace OgreMaze.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
    {
        private readonly ISwampNavigator _swampNavigator;
        private readonly IMapService _mapService;

        private int _generateMapHeight;
        private int _generateMapWidth;
        private bool _pathFound;

        public MainWindowViewModel(ISwampNavigator swampNavigator, IMazeControlViewModel mazeControlViewModel, IMapService mapService)
        {
            _swampNavigator = swampNavigator;
            _mapService = mapService;
            MazeControlViewModel = mazeControlViewModel;
            GenerateMapCommand = new RelayCommand(OnGenerateMap);
            ShowPathCommand = new RelayCommand(OnShowPath, () => PathFound);
            GenerateMapWidth = 20;
            GenerateMapHeight = 20;
        }

        public IMazeControlViewModel MazeControlViewModel { get; private set; }


        public bool PathFound
        {
            get { return _pathFound; }
            private set
            {
                _pathFound = value;
                OnPropertyChanged();
            }
        }

        public int GenerateMapHeight
        {
            get { return _generateMapHeight; }
            set
            {
                _generateMapHeight = value;
                OnPropertyChanged();
            }
        }

        public int GenerateMapWidth
        {
            get { return _generateMapWidth; }
            set
            {
                _generateMapWidth = value;
                OnPropertyChanged();
            }
        }

        public ICommand GenerateMapCommand { get; private set; }
        public ICommand ShowPathCommand { get; private set; }

        private void OnGenerateMap()
        {
            PathFound = _swampNavigator.GenerateMapAndNavigate(GenerateMapWidth, GenerateMapHeight);
            MazeControlViewModel.SwampTiles = (SwampTile[,])_mapService.Map.Clone();
            MazeControlViewModel.ShowPath = false;
        }

        private void OnShowPath()
        {
            _swampNavigator.DrawPath();
            MazeControlViewModel.SwampTilesWithPath = (SwampTile[,])_mapService.Map.Clone();
            MazeControlViewModel.ShowPath = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
