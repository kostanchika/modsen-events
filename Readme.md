# ����������� �� �������

## ���-����������

��������� � �������� ���������� (modsen-events)

```
docker-compose up --build
```

� ������ ���������� 
- EventsAPI :8080
- EventsReact :3000
- PostgreSQL :5432

�� ���� http://localhost:3000 ����� �������� ����������

## �����

�� ���� /modsen-event/EventsAPI.Tests ����������� �����

� ��������� ������ �����������

```
dotnet test EventsAPI.Tests.csproj
```