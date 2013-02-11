﻿using System;
using System.Linq;
using System.Reflection;

namespace Given.Common
{
    public class TestStateManager : ITestStateManager
    {

        public Type Test { get; set; }
        public PairList<Delegate, string> Givens { get; private set; }
        public PairList<Delegate, string> Whens { get; private set; }
        public PairList<MethodInfo, StatedThen> Thens { get; private set; }
        
        public TestStateManager(object specification)
        {
            TestRunManager.CurrentTestRun.AddTest(this);
            Test = specification.GetType();
            Givens = new PairList<Delegate, string>();
            Whens = new PairList<Delegate, string>();
            Thens = new PairList<MethodInfo, StatedThen>();
        }

        public void AddGiven(string text, Delegate method)
        {
            Givens.Add(method, text);
        }

        public void AddWhen(string text, Delegate method)
        {
            Whens.Add(method, text);
        }

        public void AddThen(string text, MethodInfo method)
        {
            Thens.Add(method, new StatedThen { Name = text });
        }

        public void SetThenState(string methodName, TestState state)
        {
            Thens.First(x => x.Key.Name == methodName).Value.State = state;
        }

        public void WriteSpecification(Action<string,object[]> consoleAction = null)
        {
            consoleAction = consoleAction ?? Console.WriteLine;
            var currentPrefix = Text.Given;
            foreach (var pair in Givens)
            {
                consoleAction(Text.Print, new object[] {currentPrefix, pair.Value.Replace("_", " ")});
                currentPrefix = Text.And;
            }
            currentPrefix = Text.When;
            foreach (var pair in Whens)
            {
                consoleAction(Text.Print, new object[] {currentPrefix, pair.Value.Replace("_", " ")});
                currentPrefix = Text.And;
            }
            currentPrefix = Text.Then;
            foreach (var pair in Thens)
            {
                consoleAction(Text.Print, new object[] {currentPrefix, pair.Value.Name.Replace("_", " ")});
                currentPrefix = Text.And;
            }
        }
    }
}