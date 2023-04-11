using AutoServiceContainer;
using DataAccessLayer.EFTech.EFDataContexts;
using System;
using System.Collections.Generic;
using TestTools.AutoServiceConfigurationImplementation;

namespace Phoenix.TestTools.Infrastructure
{
    public class UnitTestSut<T> : AutoServiceCreator<T>
        where T : class
    {
        public T Sut { get; }
        public EFDataContext Context { get; set; }

        public UnitTestSut()
        {
            Sut = CreateService<T>(DataBaseType.SqlLiteDataBase);
            Context = GetContext<EFDataContext>();
        }

        public UnitTestSut(Dictionary<Type, object> mockedObjects)
        {
            MockedObjects = mockedObjects;
            Sut = CreateService<T>(DataBaseType.SqlLiteDataBase);
            Context = GetContext<EFDataContext>();
        }
    }
}
