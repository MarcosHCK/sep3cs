import os

def cambiar_extension_a_png(directorio):
    for nombre_archivo in os.listdir(directorio):
        # Ignorar los archivos que ya son .png
        if not nombre_archivo.endswith('.png'):
            ruta_completa = os.path.join(directorio, nombre_archivo)
            nombre_base = os.path.splitext(nombre_archivo)[0]  # Obtener el nombre del archivo sin la extensión
            nueva_ruta = os.path.join(directorio, nombre_base + '.png')
            os.rename(ruta_completa, nueva_ruta)

cambiar_extension_a_png(directorio_script)
