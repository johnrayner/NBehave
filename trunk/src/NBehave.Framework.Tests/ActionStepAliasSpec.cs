﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Context = NUnit.Framework.TestFixtureAttribute;
using Specification = NUnit.Framework.TestAttribute;

namespace NBehave.Narrator.Framework.Specifications
{
    public class ActionStepAliasSpec
    {
        public class ErgoAttribute : ActionStepAttribute
        {
            //Ergo = Given in latin, from http://www.freedict.com/onldict/onldict.php
            public ErgoAttribute(string tokenString)
                : base("Ergo " + tokenString)
            { }
        }

        [Context]
        public class When_registering_alias_for_ActionStepAttribute
        {
            [Specification]
            public void ShouldFindAttributesInAssembly()
            {
                var actionStepAlias = new ActionStepAlias();
                Assert.AreEqual(4, actionStepAlias.Aliases.Count);
            }

            [Specification]
            public void ShouldRegisterAndAsDefaultAliasForGiven()
            {
                var actionStepAlias = new ActionStepAlias();
                var aliases = actionStepAlias.GetAliasFor(typeof(GivenAttribute));
                Assert.That(aliases.Count(), Is.EqualTo(1));
                Assert.That(aliases.First(), Is.EqualTo("And"));
            }

            [Specification]
            public void ShouldRegisterAndAsDefaultAliasForThen()
            {
                var actionStepAlias = new ActionStepAlias();
                var aliases = actionStepAlias.GetAliasFor(typeof(ThenAttribute));
                Assert.That(aliases.Count(), Is.EqualTo(1));
                Assert.That(aliases.First(), Is.EqualTo("And"));
            }

            [Specification]
            public void ShouldRegisterAndAsDefaultAliasForErgo()
            {
                var actionStepAlias = new ActionStepAlias();
                IEnumerable<string> aliases = actionStepAlias.GetAliasFor(typeof(ErgoAttribute));
                Assert.That(aliases.Count(), Is.EqualTo(3));
                Assert.That(aliases.First(), Is.EqualTo("Tunc"));
                Assert.That(aliases.Skip(1).First(), Is.EqualTo("Deinde"));
                Assert.That(aliases.Skip(2).First(), Is.EqualTo("Mox"));

            }

            [Specification]
            public void ShouldGetAliasForTokenString()
            {
                const string tokenStringTemplate = "{0} err, I dont know latin!";
                string tokenString = string.Format(tokenStringTemplate, "Ergo");
                var actionStepAlias = new ActionStepAlias();
                IEnumerable<string> aliases = actionStepAlias.GetAliasForTokenString(tokenString);
                Assert.That(aliases.Count(), Is.EqualTo(3));
                Assert.That(aliases.First(), Is.EqualTo(string.Format(tokenStringTemplate, "Tunc")));
                Assert.That(aliases.Skip(1).First(), Is.EqualTo(string.Format(tokenStringTemplate, "Deinde")));
                Assert.That(aliases.Skip(2).First(), Is.EqualTo(string.Format(tokenStringTemplate, "Mox")));
            }

            [Specification]
            public void ShouldGetDefaultAliasForGiven()
            {
                const string tokenString = "Given something";
                var actionStepAlias = new ActionStepAlias();
                IEnumerable<string> aliases = actionStepAlias.GetAliasForTokenString(tokenString);
                Assert.That(aliases.Count(), Is.EqualTo(1));
                Assert.That(aliases.First(), Is.EqualTo("And something"));
            }
        }

        [Context]
        public class When_manually_registering_alias_for_ActionStep
        {
            [Specification]
            public void ShouldRegisterAndAsAliasForGiven()
            {
                var actionStepAlias = new ActionStepAlias();
                actionStepAlias.AddDefaultAlias(new[] { "Foo" }, "Given");
                var aliases = actionStepAlias.GetAliasFor(typeof(GivenAttribute));
                Assert.That(aliases, Has.Member("Foo"));
            }
        }

        [Context]
        public class When_manually_registering_alias_for_ActionStep_multiple_times
        {
            [Specification]
            public void ShouldRegisterTheSameAliasForGivenOnlyOnce()
            {
                var actionStepAlias = new ActionStepAlias();
                actionStepAlias.AddDefaultAlias(new[] { "Foo" }, "Given");
                actionStepAlias.AddDefaultAlias(new[] { "Foo" }, "Given");
                var aliases = actionStepAlias.GetAliasFor(typeof(GivenAttribute));
                Assert.That(aliases.Count(), Is.EqualTo(2));
            }
        }
    }

}