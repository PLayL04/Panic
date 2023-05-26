using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Panic
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Discipline selectedDiscipline;
        public Discipline SelectedDiscipline
        { 
            get => selectedDiscipline;
            set
            {
                selectedDiscipline = value;
                OnPropertyChanged("SelectedDiscipline");
            }
        }
        public ObservableCollection<Discipline> ViewDisciplines { get; set; }
        public List<Discipline> disciplines { get; set; }

        private string title;
        public string Title { get => title; set => title = value; }
        private bool isPassed;
        public bool IsPassed { get => isPassed; set => isPassed = value; }

        IDialogService dialogService;
        IFileService fileService;

        private RelayCommand openFile;
        public RelayCommand OpenFile
        {
            get
            {
                return openFile ??
                    (openFile = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (dialogService.OpenFileDialog() == true)
                            {
                                var input = fileService.Open(dialogService.FilePath);
                                disciplines.Clear();
                                foreach (var d in input)
                                    disciplines.Add(d);

                                ViewDisciplines.Clear();
                                foreach (var d in disciplines)
                                {
                                    ViewDisciplines.Add(d);
                                }
                                dialogService.ShowMessage("Файл открыт");
                            }
                        }
                        catch (Exception ex)
                        {
                            dialogService.ShowError(ex.Message);
                        }
                    }));
            }
        }

        private RelayCommand saveFile;
        public RelayCommand SaveFile
        {
            get
            {
                return saveFile ??
                  (saveFile = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.SaveFileDialog() == true)
                          {
                              fileService.Save(dialogService.FilePath, disciplines.ToList());
                              dialogService.ShowMessage("Файл сохранен");
                          }
                      }
                      catch (Exception ex)
                      {
                          dialogService.ShowError(ex.Message);
                      }
                  }));
            }
        }

        private RelayCommand removeDiscipline;
        public RelayCommand RemoveDiscipline
        {
            get
            {
                return removeDiscipline ??
                    (removeDiscipline = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (selectedDiscipline == null)
                            {
                                dialogService.ShowMessage("Ничего не выбрано");
                                return;
                            } 
                            disciplines.Remove(selectedDiscipline);
                            UpdateViewDisciplines();
                        }
                        catch (Exception ex)
                        {
                            dialogService.ShowError(ex.Message);
                        }
                    }));
            }
        }

        private RelayCommand changeStatus;
        public RelayCommand ChangeStatus
        {
            get
            {
                return changeStatus ??
                    (changeStatus = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (selectedDiscipline == null)
                            {
                                dialogService.ShowMessage("Ничего не выбрано");
                                return;
                            }
                            selectedDiscipline.IsPassed = !selectedDiscipline.IsPassed;
                            UpdateViewDisciplines();
                        }
                        catch(Exception ex)
                        {
                            dialogService.ShowError(ex.Message);
                        }
                    }));
            }
        }

        private RelayCommand addDiscipline;
        public RelayCommand AddDiscipline 
        {
            get
            {
                return addDiscipline ?? (
                    addDiscipline = new RelayCommand(obj =>
                    {
                        foreach (var discipline in disciplines)
                        {
                            if (discipline.Title == Title)
                            {
                                dialogService.ShowError($"{Title} уже сущесвует!");
                                return;
                            }
                        }
                        if (Title == null || Title == string.Empty)
                        {
                            dialogService.ShowError("Название не может быть пустым!");
                            return;
                        }
                        Discipline newD = new Discipline(Title, IsPassed);
                        disciplines.Add(newD);
                        UpdateViewDisciplines();
                        dialogService.ShowMessage($"Добавлена {newD.Title} со статусом {newD.IsPassed}");
                    }));
            }
        }

        private bool allFilter = true;
        public bool AllFilter
        {
            get { return allFilter; } 
            set { allFilter = value; UpdateViewDisciplines(); }
        }

        private bool onlyPassedFilter = false;
        public bool OnlyPassedFilter
        {
            get { return onlyPassedFilter; }
            set { onlyPassedFilter = value; UpdateViewDisciplines(); }
        }

        private bool onlyNotPassedFilter = false;
        public bool OnlyNotPassedFilter
        {
            get { return onlyNotPassedFilter; }
            set { onlyNotPassedFilter = value; UpdateViewDisciplines(); }
        }

        

        public void UpdateViewDisciplines()
        {
            ViewDisciplines.Clear();
            if (AllFilter)
            {
                foreach (var d in disciplines)
                    ViewDisciplines.Add(d);
            }
            else if (OnlyPassedFilter)
            {
                foreach (var d in disciplines)
                    if (d.IsPassed)
                        ViewDisciplines.Add(d);
            }
            else
            {
                foreach (var d in disciplines)
                    if (!d.IsPassed)
                        ViewDisciplines.Add(d);
            }
        }
        public ViewModel(IFileService fileService, IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.fileService = fileService;
            disciplines = new List<Discipline>();
            ViewDisciplines = new ObservableCollection<Discipline>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
