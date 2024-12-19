using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using calculmvvm2;
using System.Collections.ObjectModel;
using System.Runtime.ConstrainedExecution;

public class CalculatorViewModel : INotifyPropertyChanged
{
    public class CalculationHistoryItem
    {
        public string Expression { get; set; }
        public string Result { get; set; }
        public string Condition { get; set; }


        public CalculationHistoryItem(string expression, string result, string condition)
        {
            Expression = expression;
            Result = result;
            Condition = condition;
        }

        public override string ToString()
        {
            return "njk iun ";
        }
    }

    private string _currentExpression;
    private string _currentResult;

    public ObservableCollection<CalculationHistoryItem> History { get; private set; }

    public string CurrentExpression
    {
        get => _currentExpression;
        set
        {
            _currentExpression = value;
            OnPropertyChanged(nameof(CurrentExpression));
        }
    }


    public string CurrentResult
    {
        get => _currentResult;
        set
        {
            _currentResult = value;
            OnPropertyChanged(nameof(CurrentResult));
        }
    }

    public class DivideException : Exception
    {
        public DivideException(string message) : base(message) { }
    }

    private readonly CalculatorModel _model;
    private string _display;
    private double _currentValue;
    private string _currentOperation;

    public string Display
    {
        get => _display;
        set
        {
            _display = value;
            OnPropertyChanged();
        }
    }

    public CalculatorViewModel()
    {

        _model = new CalculatorModel();
        Display = "0";
        History = new ObservableCollection<CalculationHistoryItem>();
    }
   
    public ICommand NumberCommand => new RelayCommand<string>(NumberPressed);
    public ICommand OperationCommand => new RelayCommand<string>(OperationPressed);
    public ICommand ClearCommand => new RelayCommand(Clear);
    public ICommand EqualsCommand => new RelayCommand(Calculate);
    public ICommand SquareCommand => new RelayCommand(SquarePressed);
    public ICommand CubeCommand => new RelayCommand(CubePressed);
    public ICommand RootCommand => new RelayCommand<string>(RootPressed);



   
    private void OperationPressed(string operation)
    {
        if (double.TryParse(Display, out double value))
        {
            _currentValue = value;
        }

        _currentOperation = operation;
        Display = "0";
    }


    private void PowerPressed(int exponent)
    {
        if (double.TryParse(Display, out _currentValue))
        {
            double result = Math.Pow(_currentValue, exponent);
            Display = $"{_currentValue} ^ {exponent} = {result}";

            History.Add(new CalculationHistoryItem($"{_currentValue} ^ {exponent} = {result}", result.ToString(), "Успешно"));
        }
        else
        {
            Display = "Введите число";
        }
    }

    
    private void SquarePressed()
    {
        PowerPressed(2);
    }

    
    private void CubePressed()
    {
        PowerPressed(3);
    }
    private void RootPressed(string degree)
    {
        if (double.TryParse(Display, out _currentValue))
        {

            double result = Math.Sqrt(_currentValue);
            Display = $"√{_currentValue} = {result}";


            History.Add(new CalculationHistoryItem($"√{_currentValue} = {result}", result.ToString(), "Успешно"));
        }
        else
        {
            Display = "Введите число";
        }
    }

    public void NumberPressed(string number)
    {
        if (Display == "0")
            Display = number;
        else
            Display += number;

        
        if (double.TryParse(Display, out double value))
        {
            _currentValue = value; 
        }
    }



    private void Calculate()
    {
        double secondValue = double.Parse(Display);
        double result = 0;
        string condition = "";

        try
        {
            switch (_currentOperation)
            {
                case "Add":
                    result = _model.Add(_currentValue, secondValue);
                    condition = DetermineCondition($"{_currentValue} + {secondValue}", result.ToString());
                    break;
                case "Subtract":
                    result = _model.Subtract(_currentValue, secondValue);
                    condition = DetermineCondition($"{_currentValue} - {secondValue}", result.ToString());
                    break;
                case "Multiply":
                    result = _model.Multiply(_currentValue, secondValue);
                    condition = DetermineCondition($"{_currentValue} * {secondValue}", result.ToString());
                    break;
                case "Divide":
                    if (secondValue == 0)
                        throw new DivideException("я запрещаю делить на 0 ");
                    result = _model.Divide(_currentValue, secondValue);
                    condition = DetermineCondition($"{_currentValue} / {secondValue}", result.ToString());
                    break;
                case "Root":
                    if (secondValue == 0)
                        throw new DivideException("Степень корня не может быть равна нулю.");
                    result = _model.CalculateRoot(_currentValue, secondValue);
                    condition = DetermineCondition($"√{secondValue} из {_currentValue}", result.ToString());
                    break;
                case "Percent": 
                    result = (_currentValue * secondValue) / 100;
                    condition = DetermineCondition($"{secondValue}% от {_currentValue}", result.ToString());
                    break;
            }

            Display = result.ToString();
            CurrentResult = result.ToString();


            string operationSymbol = "";
            if (_currentOperation == "Add")
            {
                operationSymbol = "+";
            }
            else if (_currentOperation == "Subtract")
            {
                operationSymbol = "-";
            }
            else if (_currentOperation == "Multiply")
            {
                operationSymbol = "*";
            }
            else if (_currentOperation == "Divide")
            {
                operationSymbol = "/";
            }

            CurrentExpression = $"{_currentValue} {operationSymbol} {secondValue} = {result} ";

            History.Add(new CalculationHistoryItem(CurrentExpression, CurrentResult, condition));
        }
        catch (DivideException ex)
        {
            Display = ex.Message;
        }
        catch (FormatException)
        {
            Display = "невенрый формат";
        }
    }



    private string DetermineCondition(string expression, string result)
    {

        if (string.IsNullOrEmpty(expression) || string.IsNullOrEmpty(result))
            return "всё ок ";


        return "";
    }

    private void Clear()
    {
        Display = "0";
        CurrentExpression = string.Empty;
        CurrentResult = string.Empty;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
   
    



}

