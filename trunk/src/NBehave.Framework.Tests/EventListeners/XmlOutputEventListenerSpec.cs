﻿using System;
using System.Text;
using NBehave.Narrator.Framework;
using NBehave.Narrator.Framework.EventListeners.Xml;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Xml;
using System.IO;


namespace NBehave.Narrator.Framework.Specifications.EventListeners
{

	[TestFixture]
	public class When_Story_is_running_with_xml_listener
	{
		private XmlDocument _xmlDoc;

		[SetUp]
		public void Setup()
		{
			RunnerBase runner = new StoryRunner();
			runner.LoadAssembly(this.GetType().Assembly);
			var memStream = new MemoryStream();
			var listener = new XmlOutputEventListener(new XmlTextWriter(memStream, Encoding.UTF8));
			runner.Run(listener);
			_xmlDoc = new XmlDocument();
			memStream.Seek(0, 0);
			_xmlDoc.Load(memStream);
		}

		[Test]
		public void Should_have_xml_in_the_specified_xml_writer()
		{
			Assert.IsNotNull(_xmlDoc.SelectSingleNode("results"));
		}

		[Test]
		public void Results_node_should_have_date_and_time_attributes()
		{
			Assert.AreEqual(DateTime.Today.ToShortDateString(), _xmlDoc.SelectSingleNode("results").Attributes["date"].Value);
			Assert.IsNotNull(_xmlDoc.SelectSingleNode("results").Attributes["time"].Value);
		}

		[Test]
		public void Results_node_hould_have_name_attribute()
		{
			Assert.IsTrue(_xmlDoc.SelectSingleNode("results").Attributes["name"].Value.Trim().StartsWith("NBehave"));
		}
		
		[Test]
		public void Results_node_should_have_version_attribute()
		{
			Assert.IsNotNull(_xmlDoc.SelectSingleNode("results").Attributes["version"].Value);
		}
		
		[Test]
		public void Results_node_should_have_two_themes()
		{
			var outcome = _xmlDoc.SelectSingleNode("results").Attributes["themes"].Value;
			Assert.That(int.Parse(outcome), Is.EqualTo(2));
		}

		[Test]
		public void Should_have_three_scenarios_in_results_node()
		{
			var outcome = _xmlDoc.SelectSingleNode("results").Attributes["scenarios"].Value;
			Assert.That(int.Parse(outcome), Is.EqualTo(3));
		}

		[Test]
		public void Theme_T1_should_have_two_scenarios()
		{
			var outcome = _xmlDoc.SelectSingleNode("results/theme[@name='T1']").Attributes["scenarios"].Value;
			Assert.That(int.Parse(outcome), Is.EqualTo(2));
		}
		
		[Test]
		public void Theme_T2_should_have_one_scenario()
		{
			var outcome = _xmlDoc.SelectSingleNode("results/theme[@name='T2']").Attributes["scenarios"].Value;
			Assert.That(int.Parse(outcome), Is.EqualTo(1));
		}

		[Test]
		public void Theme_nodes_should_contain_attribute_about_execution_time()
		{
			Assert.IsNotNull(_xmlDoc.SelectSingleNode("results/theme[@name='T1']").Attributes["time"].Value);
		}

		[Test]
		public void Theme_node_should_have_zero_pending_scenarios()
		{
			Assert.AreEqual("0", _xmlDoc.SelectSingleNode(@"results/theme[@name='T1']").Attributes["scenariosPending"].Value);
		}
		
		[Test]
		public void Theme_node_should_have_zero_failed_scenarios()
		{
			Assert.AreEqual("0", _xmlDoc.SelectSingleNode(@"results/theme[@name='T1']").Attributes["scenariosFailed"].Value);
		}

		[Test]
		public void Story_node_should_have_a_name_attribute()
		{
			Assert.IsNotNull(_xmlDoc.SelectSingleNode("//story[@name='S1']"));
		}

		[Test]
		public void Story_should_have_a_narrative_child_element()
		{
			Assert.IsNotNull(_xmlDoc.SelectSingleNode("results/theme[@name='T1']/stories/story[@name='S1']/narrative"));
			Assert.AreEqual("As a somebody" + Environment.NewLine +
			                "I want this to work" + Environment.NewLine +
			                "So that I can do something else" + Environment.NewLine,
			                _xmlDoc.SelectSingleNode("results/theme/stories/story/narrative").InnerText);
		}
		
		[Test]
		public void Story_node_should_have_summary()
		{
			Assert.AreEqual("2", _xmlDoc.SelectSingleNode(@"results/theme[@name='T1']/stories/story").Attributes["scenarios"].Value);
			Assert.AreEqual("0", _xmlDoc.SelectSingleNode(@"results/theme[@name='T1']/stories/story").Attributes["scenariosFailed"].Value);
			Assert.AreEqual("0", _xmlDoc.SelectSingleNode(@"results/theme[@name='T1']/stories/story").Attributes["scenariosPending"].Value);
		}
	
		[Test]
		public void Story_node_should_have_one_scenario_node_per_scenario_in_story()
		{
			var node = _xmlDoc.SelectNodes(@"//stories/story[@name='S1']/scenarios/scenario");
			Assert.IsNotNull(node);
			Assert.AreEqual(2, node.Count);
		}
		
		[Test]
		public void Scenario_node_should_have_text_subnode()
		{
			var node = _xmlDoc.SelectSingleNode(@"//scenario[@name='SC1']/text");
			Assert.IsNotNull(node);
			Assert.IsNotNull(node.InnerText);
		}
	}
}
