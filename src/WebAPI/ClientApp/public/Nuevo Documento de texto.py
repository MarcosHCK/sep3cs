import os

# Directorio donde se encuentran las imágenes
image_directory = "./cards"

# Lista para guardar todas las líneas de código generadas
addCard_lines = []

# Iterar sobre todos los archivos en el directorio de imágenes
for filename in os.listdir(image_directory):
    # Remover la extensión .png del nombre del archivo
    card_name = os.path.splitext(filename)[0]
    
    # Crear la línea de código para agregar la carta
    # (Aquí estoy asumiendo que todas las cartas son de tipo TroopCard y estoy usando valores ficticios para los otros parámetros. Deberías reemplazar estos valores por los correctos para cada carta)
    addCard_line = f'AddCard<TroopCard>(context, "{card_name}.png", "{card_name} description", 1, 1, "{card_name}", Quality.Normal, 100, 100, 1);'
    
    # Agregar la línea de código a la lista
    addCard_lines.append(addCard_line)

# Imprimir todas las líneas de código
for line in addCard_lines:
    print(line)
