# kr.bbon.Data Package


[![](https://img.shields.io/nuget/v/kr.bbon.Data)](https://www.nuget.org/packages/kr.bbon.Data) [![](https://img.shields.io/nuget/dt/kr.bbon.Data)](https://www.nuget.org/packages/kr.bbon.Data) [![publish to nuget](https://github.com/bbonkr/kr.bbon.Data/actions/workflows/dotnet.yaml/badge.svg)](https://github.com/bbonkr/kr.bbon.Data/actions/workflows/dotnet.yaml)

## 📢 Overview

## 🌈 Namespace

### kr.bbon.Data

DbContext 기반이 되는 AppDbContextBase 클래스와 IEntityTypeConfiguration 인터페이스 일부가 구현된 EntityTypeConfigurationBase 클래스를 제공합니다.

AppDbContextBase 는 Soft deletion 을 지원하는 엔티티에 대한 처리 등이 포함되어 있습니다.

작성하려는 DbContext 클래스는 AppDbContextBase 클래스 기반으로 필요한 기능을 확장합니다.

EntityTypeConfigurationBase 는 kr.bbon.Data.Abstractions.Entities 네임스페이스에서 제공되는 형식으로 구현 또는 확장된 엔티티에 대한 IEntityTypeConfiguration 인터페이스의 일부 구현이 제공됩니다.

### kr.bbon.Data.Repositories

kr.bbon.Data.Abstractions.IRepository 인터페이스를 구현하는 레포지토리 클래스가 제공됩니다.

데이터 쿼리를 위한 기능은 Specification 패턴을 사용하게 준비되어 있습니다.

### kr.bbon.Data.Services

Unit of work 구현의 저의 해석에 대한 DataServiceBase 클래스가 제공됩니다.

IRepository 인터페이스의 구현 형식을 필드로 추가해서 사용합니다.

### kr.bbon.Data.Extensions.DependencyInjection

의존성 구성을 위한 IServiceCollection 확장과 Specification 패턴 구현을 위한 IQueryable 확장이 제공됩니다.

## 🎯 Features

### AppDbContextBase 

DbContext 기반 클래스입니다.

DbSet<TEntity> 필드를 추가하고, OnModelCreating 메서드를 재정의해서 EntityTypeConfiguration 이 구현된 어셈블리를 활용하게 계획되어 있습니다.

### EntityTypeConfigurationBase 

kr.bbon.Data.Abstractions.Entities 네임스페이스에 준비된 엔티티 기반 형식으로 엔티티 클래스를 정의하면, 미리 정의된 기능에 대한 엔티티 형식 구성의 준비된 구성을 제공합니다.

### RepositoryBase

레포지토리 패턴을 구현하는 기반 클래스를 제공합니다.

데이터 출력은 Specification 패턴으로 쿼리를 정의할 수 있게 SpecificationBase 클래스를 사용합니다.

데이터 입력은 EntityFrameworkCore의 기본 기능을 사용하며, 특이사항은 없습니다.

### DataServiceBase 

Unit of work 구현에 대한 저의 해석으로 기반 클래스를 제공합니다.

데이터 처리에 필요한 Repository 는 생성자 주입으로 구성하고, 각 Repository 는 필드로 정의해서 외부에서 접근할 수 있게 구현하게 준비되어 있습니다.

DataService 에 DbContext 가 있어, 여러 저장소의 트랜잭션을 하나로 처리할 수 있습니다.

DataServie 구현 프로젝트와 도메인 기능 프로젝트를 분리할 수 있게 구성하는 것을 추천합니다.

