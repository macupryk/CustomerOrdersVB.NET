import os
import sqlite3   
import codecs
      
def create_northwind_db():
  if os.path.isfile(r'..\src\App_Data\northwind.db'):
      print 'This script will create the Norhtwind database for you.' + \
            'If you want to re-create the database, please delete the ' + \
            'existing file if you want to re-create it.'
      try:
          os.system('pause')                                #windows
      except:
          os.system('read -p "Press any key to continue"')  #linux
  else:
      print 'Creating \'Northwind.db\'.  Please wait as this may take up to a minute to complete.'      
      query = codecs.open('Northwind.Sqlite3.sql', 'r', encoding='iso-8859-1').read()
      conn = sqlite3.connect(r'..\src\App_Data\northwind.db') 
      cur = conn.cursor()
      cur.executescript(query)
      conn.commit()
      cur.close()
      conn.close()  

if __name__ == '__main__':
    create_northwind_db()
  
