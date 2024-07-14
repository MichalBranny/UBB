import pygame
import math

# Inicjalizacja Pygame
pygame.init()
win = pygame.display.set_mode((600, 600))
pygame.display.set_caption("Wielokąt - Jedenastokąt")

# Definiowanie kolorów
CZARNY = (0, 0, 0)
BIAŁY = (255, 255, 255)

# Funkcja do rysowania wielokąta
def draw_polygon(surface, color, num_sides, radius, position, rotation_angle=0):
    points = []
    for i in range(num_sides):
        angle = math.radians(float(i) / num_sides * 360 + rotation_angle)
        x = position[0] + radius * math.cos(angle)
        y = position[1] + radius * math.sin(angle)
        points.append([x, y])
    pygame.draw.polygon(surface, color, points)

# Początkowe wartości dla położenia, promienia i kąta obrotu
position = (300, 300)
radius = 150
rotation_angle = 0

# Główna pętla gry
run = True
while run:
    win.fill(BIAŁY)
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            run = False
        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_1:
                # Przesunięcie w prawo
                position = (position[0] + 10, position[1])
            elif event.key == pygame.K_2:
                # Przesunięcie w lewo
                position = (position[0] - 10, position[1])
            elif event.key == pygame.K_3:
                # Przesunięcie w dół
                position = (position[0], position[1] + 10)
            elif event.key == pygame.K_4:
                # Przesunięcie w górę
                position = (position[0], position[1] - 10)
            elif event.key == pygame.K_5:
                # Zwiększenie promienia (skalowanie)
                radius += 10
            elif event.key == pygame.K_6:
                # Zmniejszenie promienia (skalowanie)
                radius -= 10
            elif event.key == pygame.K_7:
                # Obrót w prawo
                rotation_angle += 10
            elif event.key == pygame.K_8:
                # Obrót w lewo
                rotation_angle -= 10
            elif event.key == pygame.K_9:
                # Inny typ transformacji (przykładowo resetowanie)
                position = (300, 300)
                radius = 150
                rotation_angle = 0

    # Rysowanie jedenastokąta
    draw_polygon(win, CZARNY, 11, radius, position, rotation_angle)

    pygame.display.update()

pygame.quit()
