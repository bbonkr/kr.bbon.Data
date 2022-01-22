# kr.bbon.Data.Abstractions Pacakge


[![](https://img.shields.io/nuget/v/kr.bbon.Data.Abstractions)](https://www.nuget.org/packages/kr.bbon.Data.Abstractions) [![](https://img.shields.io/nuget/dt/kr.bbon.Data.Abstractions)](https://www.nuget.org/packages/kr.bbon.Data.Abstractions) [![publish to nuget](https://github.com/bbonkr/kr.bbon.Data/actions/workflows/build-tag.yaml/badge.svg)](https://github.com/bbonkr/kr.bbon.Data/actions/workflows/build-tag.yaml)

## 📢 Overview

kr.bbon.Data 패키지의 기능의 추상화 계층을 제공합니다.

데이터 저장을 [kr.bbon.Data](./kr.bbon.Data.md) 패키지를 사용해서 구현한 경우 응용프로그램 계층과 데이터 저장 계층의 의존을 제거하기 위해 사용됩니다.

## 🌈 Namespace

### kr.bbon.Data.Abstractions

DataService 클래스와 Repository 클래스의 추상화입니다.

### kr.bbon.Data.Abstractions.Entities

Entity 클래스의 추상화입니다.

## 🎯 Features

### Repository

[Repository 패턴](https://docs.microsoft.com/ko-kr/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design) 구현을 위한 정의입니다.

### DataService

Repository 를 통합해서, 하나의 접근 통로를 제공하는 서비스 계층을 구현하기 위한 정의입니다.

[Unit of Work 의 구현](https://docs.microsoft.com/ko-kr/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)을 조금 다르게 생각했습니다.

### Specification

Repository 구현을 일반화하기 위해 쿼리 실행에 [Specification 패턴](https://en.wikipedia.org/wiki/Specification_pattern)을 적용하기 위한 정의입니다.

### Entity

엔티티 정의를 위한 기반을 제공합니다.

해당 기능을 확장한 엔티티는 준비된 기능이 제공됩니다.

준비된 기능:

* 데이터 작성시각, 변경시각 필드를 갖는 엔티티
* 기본 식별자를 갖는 엔티티
* Soft deletion[^soft-deletion] 기능을 제공하는 엔티티


## 참조

[^soft-deletion]: 데이터베이스에서 행을 삭제하지 않고, 삭제된 플래그로 데이터 삭제여부를 제어합니다. 데이터 조회시 항상 삭제 플래그가 포함되어야 합니다.