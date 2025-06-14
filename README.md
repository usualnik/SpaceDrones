# Space Drones

![](Assets/Art/SpaceDrones.gif)

Я получил задание в воскресенье, но сразу уведомил в чате, что смогу приступить только с понедельника.
Ввиду этого и ограниченного времени, некоторые пункты не были реализованы, но я постарался разработать качественную и расширяемую архитектуру, нацеленную на масштабируемость.

## Реализовано:
- Поиск ближайшего свободного ресурса
- Движение дрона к ресурсу (включая поиск пути и избегание препятствий)
- Дрон собирает ресурсы в течении 2-ух секунд, ресурс исчезает
- Возврат дрона на базу
- Выгрузка ресурса + простейшая система частиц при выгрузке
- Цикл повторяется
## Не реализовано:
- Избегание столкновений между дронами
- Отсутствует UI

Причина: нехватка времени из-за смещённого старта.

## Архитектура

### Ресурсы спавнятся по всей карте через обычный спавнер с InvokeRepeating(). Информацию о ресурсах централизованно хранит ResourceManager.cs
 
1) Скрипт Resource.cs реализует интерфейс ICollectable, что позволяет его собирать.
  
### Обе базы имеют функционал для спавна дронов и инвентарь для хранения ресурсов, собранных дронами

### Дроны управляются централизованным FSM через скрипт Drone.cs, который делегирует поведение на зависимые компоненты:
1) DroneMovement.cs // Выполняет движение к цели и проверку препятствий перед собой, так же валидирует движение, чтобы дрон знал на какую базу возвращаться. Уведомляет об успешности передвижения скрипт Drone через callback
2) DroneResourceScanner.cs // Ищет ближайший незанятый ресурс из списка, который хранит ResourceManager.cs, уведомляет об успешности скрипт Drone через callback
3) DroneResourceCollector.cs // Собирает ресурс, передавая информацию о нем в DroneInventory.cs, уведомляет об успешности скрипт Drone через callback
4) DroneInventory.cs // Хранит информацию о ресурсах, находящихся в дроне, имеет функционал по выгрузке ресурсов на базу в скрипт BaseInventory.cs
5) DroneAnimationHandler.cs // Отвечает за анимацию, проигрывает particle system, при выгрузке ресурсов на базу.
