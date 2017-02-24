This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

[![Build status](https://ci.appveyor.com/api/projects/status/abtasjr2kmehollq?svg=true)](https://ci.appveyor.com/project/orellabac/aspnet-redis-providers)


Why we patched the ASP.NET Redis Provider
========================================

The reason has nothing to do with the Redis Provider but instead is an issue with the `SessionStateItemCollection` class.
On System.Web this is the base class used for sessions. 
Sadly there is an `ISessionStateItemCollection` but the class is sealed and its base class lacks some virtual methods.

Another strange design choice was that the `SessionStateItemCollection` extends the `NameObjectCollectionBase` instead of a dictionary.
This class is meant for collections of keys and values, but not necessary unique pairs. This causes the issue that the `Remove` method which is as follows:
```C#
protected void BaseRemove(string name)
{
	if (this._readOnly)
	{
		throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
	}
	if (name != null)
	{
		this._entriesTable.Remove(name);
		for (int i = this._entriesArray.Count - 1; i >= 0; i--)
		{
			if (this._keyComparer.Equals(name, this.BaseGetKey(i)))
			{
				this._entriesArray.RemoveAt(i);
			}
		}
	}
	else
	{
		this._nullKeyEntry = null;
		for (int j = this._entriesArray.Count - 1; j >= 0; j--)
		{
			if (this.BaseGetKey(j) == null)
			{
				this._entriesArray.RemoveAt(j);
			}
		}
	}
	this._version++;
}
```

Performs an iteration thru all its entries, adding a lot of overhead on remove operations.
This branch just patches this behavior by providing an `ISessionStateItemCollection` that relies on an `OrderedDictionary` instead which has a better performance.


ASP.NET Redis Providers
=======================


This repository contains code for Session State and Output Cache providers for Redis.
1) Azure Redis Session State Provider can be use to store your session state in a Redis Cache rather than in-memory or in a SQL Server database.
2) Azure Redis Output Cache Provider is an out-of-process storage mechanism for output cache data. This data is specifically for full HTTP responses (page output caching). 

## Documentation

See [Azure Redis Session State Provider Documentation](https://azure.microsoft.com/documentation/articles/cache-aspnet-session-state-provider/) and [Azure Redis Output Cache Provider Documentation](https://azure.microsoft.com/documentation/articles/cache-aspnet-output-cache-provider/)

## License

This project is under the umbrella of the [.NET Foundation](http://www.dotnetfoundation.org/) and is licensed under [the MIT License](https://github.com/Azure/aspnet-redis-providers/blob/master/License.txt)

## Build and Test
Open Microsoft.CacheProviders.sln with Visual Studio. You should install [xUnit.net runner for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/463c5987-f82b-46c8-a97e-b1cde42b9099) in visual studio using "TOOLS -> Extensions and Updates..."
Right click on solution in "Solution Explorer" and click on "Build Solution" in visual studio to build all projects. Open "Test Explorer" in visual studio and run all tests by clicking on "Run All".

## Release Notes
[Release Notes](https://github.com/Azure/aspnet-redis-providers/wiki/Release-Notes)

## Questions?

* [Azure Cache Forum](https://social.msdn.microsoft.com/Forums/en-US/home?forum=azurecache)
* [StackOverflow for Azure Redis Cache](http://stackoverflow.com/questions/tagged/azure-redis-cache)
