using AutoServiceContainer;
using DataAccessLayer.EFTech.EFDataContexts;
using System;
using System.Collections.Generic;

namespace TestTools.AutoServiceConfigurationImplementation
{
    public class UnitTestServiceCreator<T>
        : AutoServiceCreator<T> where T : class
    {
        public UnitTestServiceCreator()
        {
            Sut = CreateService<T>(dataBase: DataBaseType.SqlLiteDataBase);
            Context = GetContext<EFDataContext>();
        }
        public UnitTestServiceCreator(Dictionary<Type, object> mockedObjects)
        {
            MockedObjects = mockedObjects;
            Sut = CreateService<T>(dataBase: DataBaseType.SqlLiteDataBase);
            Context = GetContext<EFDataContext>();
        }

        public T Sut { get; set; }
        public EFDataContext Context { get; set; }
    }
}