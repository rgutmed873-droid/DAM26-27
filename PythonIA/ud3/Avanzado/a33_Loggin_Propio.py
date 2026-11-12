#Actividad 33


import loggin

print("Iniciando sistema...")
# Registramos un aviso normal
loggin.registrar("info", "La aplicación acaba de arrancar correctamente.")

datos = ["10", "5", "cero", "8"]

for texto in datos:
    try:
        mitad = int(texto) / 2
        loggin.registrar("info", f"Se calculó con éxito la mitad de {texto}: {mitad}")
    except ValueError:
        # Si falla, registramos un error en el log
        loggin.registrar("error", f"El usuario intentó procesar texto no válido: '{texto}'")

print("Proceso terminado. Revisa tu carpeta para ver el nuevo archivo 'aplicacion.log'.")