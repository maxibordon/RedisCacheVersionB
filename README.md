## Instrucciones

### Las siguientes dependencias son necesarias para implementar Redis como cache

- StackExchange.Redis

### Instalar Redis en local:

- Instalar docker en windows https://docs.docker.com/desktop/windows/install/
- Ejecutar desde un powershell **docker run --name redis -d -p 5003:6379 redis redis-server --requirepass "PASSWORD"** . Si todo fue ok, se deberá devolver un container id como   por ejemplo **2dabade3f5888e63e94cb692faa7fe925072ddb5e1517f1d3ffdfb3faf98dc24**  
- Para ver redis en local: instalar para windows https://github.com/qishibo/AnotherRedisDesktopManager/releases
