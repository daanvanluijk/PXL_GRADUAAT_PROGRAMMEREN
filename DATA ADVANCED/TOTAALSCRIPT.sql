-- Script om heel de database te genereren (HR applicatie)






PROMPT
PROMPT Specify password for system as parameter 1:
DEFINE pass = &1

----------------------
-- CREATING HR USER --
----------------------

CONNECT system/&pass;

DROP USER HR CASCADE;

CREATE USER HR IDENTIFIED BY &pass
DEFAULT TABLESPACE USERS
QUOTA UNLIMITED ON USERS;

COMMIT;

------------------------
-- GRANTING HR RIGHTS --
------------------------

CONNECT system/&pass;

GRANT CREATE SESSION TO HR;
GRANT CREATE TABLE, CREATE VIEW, CREATE TRIGGER, CREATE SEQUENCE TO HR;

COMMIT;

------------------------
-- CREATING HR TABLES --
------------------------

CONNECT HR/&pass;

Prompt ******  Creating REGIONS table ....

CREATE TABLE regions
    ( region_id      NUMBER 
       CONSTRAINT  region_id_nn NOT NULL 
    , region_name    VARCHAR2(25) 
    );

CREATE UNIQUE INDEX reg_id_pk
ON regions (region_id);

ALTER TABLE regions
ADD ( CONSTRAINT reg_id_pk
       		 PRIMARY KEY (region_id)
    ) ;

Prompt ******  Creating COUNTRIES table ....

CREATE TABLE countries 
    ( country_id      CHAR(2) 
       CONSTRAINT  country_id_nn NOT NULL 
    , country_name    VARCHAR2(40) 
    , region_id       NUMBER 
    , CONSTRAINT     country_c_id_pk 
        	     PRIMARY KEY (country_id) 
    ) 
    ORGANIZATION INDEX; 

ALTER TABLE countries
ADD ( CONSTRAINT countr_reg_fk
        	 FOREIGN KEY (region_id)
          	  REFERENCES regions(region_id) 
    ) ;

Prompt ******  Creating LOCATIONS table ....

CREATE TABLE locations
    ( location_id    NUMBER(4)
    , street_address VARCHAR2(40)
    , postal_code    VARCHAR2(12)
    , city       VARCHAR2(30)
	CONSTRAINT     loc_city_nn  NOT NULL
    , state_province VARCHAR2(25)
    , country_id     CHAR(2)
    ) ;

CREATE UNIQUE INDEX loc_id_pk
ON locations (location_id) ;

ALTER TABLE locations
ADD ( CONSTRAINT loc_id_pk
       		 PRIMARY KEY (location_id)
    , CONSTRAINT loc_c_id_fk
       		 FOREIGN KEY (country_id)
        	  REFERENCES countries(country_id) 
    ) ;

CREATE SEQUENCE locations_seq
 START WITH     3300
 INCREMENT BY   100
 MAXVALUE       9900
 NOCACHE
 NOCYCLE;

Prompt ******  Creating DEPARTMENTS table ....

CREATE TABLE departments
    ( department_id    NUMBER(4)
    , department_name  VARCHAR2(30)
	CONSTRAINT  dept_name_nn  NOT NULL
    , manager_id       NUMBER(6)
    , location_id      NUMBER(4)
    ) ;

CREATE UNIQUE INDEX dept_id_pk
ON departments (department_id) ;

ALTER TABLE departments
ADD ( CONSTRAINT dept_id_pk
       		 PRIMARY KEY (department_id)
    , CONSTRAINT dept_loc_fk
       		 FOREIGN KEY (location_id)
        	  REFERENCES locations (location_id)
     ) ;

CREATE SEQUENCE departments_seq
 START WITH     280
 INCREMENT BY   10
 MAXVALUE       9990
 NOCACHE
 NOCYCLE;

Prompt ******  Creating JOBS table ....

CREATE TABLE jobs
    ( job_id         VARCHAR2(10)
    , job_title      VARCHAR2(35)
	CONSTRAINT     job_title_nn  NOT NULL
    , min_salary     NUMBER(6)
    , max_salary     NUMBER(6)
    ) ;

CREATE UNIQUE INDEX job_id_pk 
ON jobs (job_id) ;

ALTER TABLE jobs
ADD ( CONSTRAINT job_id_pk
      		 PRIMARY KEY(job_id)
    ) ;

Prompt ******  Creating EMPLOYEES table ....

CREATE TABLE employees
    ( employee_id    NUMBER(6)
    , first_name     VARCHAR2(20)
    , last_name      VARCHAR2(25)
	 CONSTRAINT     emp_last_name_nn  NOT NULL
    , email          VARCHAR2(25)
	CONSTRAINT     emp_email_nn  NOT NULL
    , phone_number   VARCHAR2(20)
    , hire_date      DATE
	CONSTRAINT     emp_hire_date_nn  NOT NULL
    , job_id         VARCHAR2(10)
	CONSTRAINT     emp_job_nn  NOT NULL
    , salary         NUMBER(8,2)
    , commission_pct NUMBER(2,2)
    , manager_id     NUMBER(6)
    , department_id  NUMBER(4)
    , CONSTRAINT     emp_salary_min
                     CHECK (salary > 0) 
    , CONSTRAINT     emp_email_uk
                     UNIQUE (email)
    ) ;

CREATE UNIQUE INDEX emp_emp_id_pk
ON employees (employee_id) ;

ALTER TABLE employees
ADD ( CONSTRAINT     emp_emp_id_pk
                     PRIMARY KEY (employee_id)
    , CONSTRAINT     emp_dept_fk
                     FOREIGN KEY (department_id)
                      REFERENCES departments
    , CONSTRAINT     emp_job_fk
                     FOREIGN KEY (job_id)
                      REFERENCES jobs (job_id)
    , CONSTRAINT     emp_manager_fk
                     FOREIGN KEY (manager_id)
                      REFERENCES employees
    ) ;

ALTER TABLE departments
ADD ( CONSTRAINT dept_mgr_fk
      		 FOREIGN KEY (manager_id)
      		  REFERENCES employees (employee_id)
    ) ;

CREATE SEQUENCE employees_seq
 START WITH     207
 INCREMENT BY   1
 NOCACHE
 NOCYCLE;

Prompt ******  Creating JOB_HISTORY table ....

CREATE TABLE job_history
    ( employee_id   NUMBER(6)
	 CONSTRAINT    jhist_employee_nn  NOT NULL
    , start_date    DATE
	CONSTRAINT    jhist_start_date_nn  NOT NULL
    , end_date      DATE
	CONSTRAINT    jhist_end_date_nn  NOT NULL
    , job_id        VARCHAR2(10)
	CONSTRAINT    jhist_job_nn  NOT NULL
    , department_id NUMBER(4)
    , CONSTRAINT    jhist_date_interval
                    CHECK (end_date > start_date)
    ) ;

CREATE UNIQUE INDEX jhist_emp_id_st_date_pk 
ON job_history (employee_id, start_date) ;

ALTER TABLE job_history
ADD ( CONSTRAINT jhist_emp_id_st_date_pk
      PRIMARY KEY (employee_id, start_date)
    , CONSTRAINT     jhist_job_fk
                     FOREIGN KEY (job_id)
                     REFERENCES jobs
    , CONSTRAINT     jhist_emp_fk
                     FOREIGN KEY (employee_id)
                     REFERENCES employees
    , CONSTRAINT     jhist_dept_fk
                     FOREIGN KEY (department_id)
                     REFERENCES departments
    ) ;

Prompt ******  Creating EMP_DETAILS_VIEW view ...

CREATE OR REPLACE VIEW emp_details_view
  (employee_id,
   job_id,
   manager_id,
   department_id,
   location_id,
   country_id,
   first_name,
   last_name,
   salary,
   commission_pct,
   department_name,
   job_title,
   city,
   state_province,
   country_name,
   region_name)
AS SELECT
  e.employee_id, 
  e.job_id, 
  e.manager_id, 
  e.department_id,
  d.location_id,
  l.country_id,
  e.first_name,
  e.last_name,
  e.salary,
  e.commission_pct,
  d.department_name,
  j.job_title,
  l.city,
  l.state_province,
  c.country_name,
  r.region_name
FROM
  employees e,
  departments d,
  jobs j,
  locations l,
  countries c,
  regions r
WHERE e.department_id = d.department_id
  AND d.location_id = l.location_id
  AND l.country_id = c.country_id
  AND c.region_id = r.region_id
  AND j.job_id = e.job_id 
WITH READ ONLY;

COMMIT;

-----------------------
-- FILLING HR TABLES --
-----------------------

SET VERIFY OFF
ALTER SESSION SET NLS_LANGUAGE=American; 

REM ***************************insert data into the REGIONS table

Prompt ******  Populating REGIONS table ....

INSERT INTO regions VALUES 
        ( 1
        , 'Europe' 
        );

INSERT INTO regions VALUES 
        ( 2
        , 'Americas' 
        );

INSERT INTO regions VALUES 
        ( 3
        , 'Asia' 
        );

INSERT INTO regions VALUES 
        ( 4
        , 'Middle East and Africa' 
        );

REM ***************************insert data into the COUNTRIES table

Prompt ******  Populating COUNTIRES table ....

INSERT INTO countries VALUES 
        ( 'IT'
        , 'Italy'
        , 1 
        );

INSERT INTO countries VALUES 
        ( 'JP'
        , 'Japan'
	, 3 
        );

INSERT INTO countries VALUES 
        ( 'US'
        , 'United States of America'
        , 2 
        );

INSERT INTO countries VALUES 
        ( 'CA'
        , 'Canada'
        , 2 
        );

INSERT INTO countries VALUES 
        ( 'CN'
        , 'China'
        , 3 
        );

INSERT INTO countries VALUES 
        ( 'IN'
        , 'India'
        , 3 
        );

INSERT INTO countries VALUES 
        ( 'AU'
        , 'Australia'
        , 3 
        );

INSERT INTO countries VALUES 
        ( 'ZW'
        , 'Zimbabwe'
        , 4 
        );

INSERT INTO countries VALUES 
        ( 'SG'
        , 'Singapore'
        , 3 
        );

INSERT INTO countries VALUES 
        ( 'UK'
        , 'United Kingdom'
        , 1 
        );

INSERT INTO countries VALUES 
        ( 'FR'
        , 'France'
        , 1 
        );

INSERT INTO countries VALUES 
        ( 'DE'
        , 'Germany'
        , 1 
        );

INSERT INTO countries VALUES 
        ( 'ZM'
        , 'Zambia'
        , 4 
        );

INSERT INTO countries VALUES 
        ( 'EG'
        , 'Egypt'
        , 4 
        );

INSERT INTO countries VALUES 
        ( 'BR'
        , 'Brazil'
        , 2 
        );

INSERT INTO countries VALUES 
        ( 'CH'
        , 'Switzerland'
        , 1 
        );

INSERT INTO countries VALUES 
        ( 'NL'
        , 'Netherlands'
        , 1 
        );

INSERT INTO countries VALUES 
        ( 'MX'
        , 'Mexico'
        , 2 
        );

INSERT INTO countries VALUES 
        ( 'KW'
        , 'Kuwait'
        , 4 
        );

INSERT INTO countries VALUES 
        ( 'IL'
        , 'Israel'
        , 4 
        );

INSERT INTO countries VALUES 
        ( 'DK'
        , 'Denmark'
        , 1 
        );

INSERT INTO countries VALUES 
        ( 'ML'
        , 'Malaysia'
        , 3 
        );

INSERT INTO countries VALUES 
        ( 'NG'
        , 'Nigeria'
        , 4 
        );

INSERT INTO countries VALUES 
        ( 'AR'
        , 'Argentina'
        , 2 
        );

INSERT INTO countries VALUES 
        ( 'BE'
        , 'Belgium'
        , 1 
        );


REM ***************************insert data into the LOCATIONS table

Prompt ******  Populating LOCATIONS table ....

INSERT INTO locations VALUES 
        ( 1000 
        , '1297 Via Cola di Rie'
        , '00989'
        , 'Roma'
        , NULL
        , 'IT'
        );

INSERT INTO locations VALUES 
        ( 1100 
        , '93091 Calle della Testa'
        , '10934'
        , 'Venice'
        , NULL
        , 'IT'
        );

INSERT INTO locations VALUES 
        ( 1200 
        , '2017 Shinjuku-ku'
        , '1689'
        , 'Tokyo'
        , 'Tokyo Prefecture'
        , 'JP'
        );

INSERT INTO locations VALUES 
        ( 1300 
        , '9450 Kamiya-cho'
        , '6823'
        , 'Hiroshima'
        , NULL
        , 'JP'
        );

INSERT INTO locations VALUES 
        ( 1400 
        , '2014 Jabberwocky Rd'
        , '26192'
        , 'Southlake'
        , 'Texas'
        , 'US'
        );

INSERT INTO locations VALUES 
        ( 1500 
        , '2011 Interiors Blvd'
        , '99236'
        , 'South San Francisco'
        , 'California'
        , 'US'
        );

INSERT INTO locations VALUES 
        ( 1600 
        , '2007 Zagora St'
        , '50090'
        , 'South Brunswick'
        , 'New Jersey'
        , 'US'
        );

INSERT INTO locations VALUES 
        ( 1700 
        , '2004 Charade Rd'
        , '98199'
        , 'Seattle'
        , 'Washington'
        , 'US'
        );

INSERT INTO locations VALUES 
        ( 1800 
        , '147 Spadina Ave'
        , 'M5V 2L7'
        , 'Toronto'
        , 'Ontario'
        , 'CA'
        );

INSERT INTO locations VALUES 
        ( 1900 
        , '6092 Boxwood St'
        , 'YSW 9T2'
        , 'Whitehorse'
        , 'Yukon'
        , 'CA'
        );

INSERT INTO locations VALUES 
        ( 2000 
        , '40-5-12 Laogianggen'
        , '190518'
        , 'Beijing'
        , NULL
        , 'CN'
        );

INSERT INTO locations VALUES 
        ( 2100 
        , '1298 Vileparle (E)'
        , '490231'
        , 'Bombay'
        , 'Maharashtra'
        , 'IN'
        );

INSERT INTO locations VALUES 
        ( 2200 
        , '12-98 Victoria Street'
        , '2901'
        , 'Sydney'
        , 'New South Wales'
        , 'AU'
        );

INSERT INTO locations VALUES 
        ( 2300 
        , '198 Clementi North'
        , '540198'
        , 'Singapore'
        , NULL
        , 'SG'
        );

INSERT INTO locations VALUES 
        ( 2400 
        , '8204 Arthur St'
        , NULL
        , 'London'
        , NULL
        , 'UK'
        );

INSERT INTO locations VALUES 
        ( 2500 
        , 'Magdalen Centre, The Oxford Science Park'
        , 'OX9 9ZB'
        , 'Oxford'
        , 'Oxford'
        , 'UK'
        );

INSERT INTO locations VALUES 
        ( 2600 
        , '9702 Chester Road'
        , '09629850293'
        , 'Stretford'
        , 'Manchester'
        , 'UK'
        );

INSERT INTO locations VALUES 
        ( 2700 
        , 'Schwanthalerstr. 7031'
        , '80925'
        , 'Munich'
        , 'Bavaria'
        , 'DE'
        );

INSERT INTO locations VALUES 
        ( 2800 
        , 'Rua Frei Caneca 1360 '
        , '01307-002'
        , 'Sao Paulo'
        , 'Sao Paulo'
        , 'BR'
        );

INSERT INTO locations VALUES 
        ( 2900 
        , '20 Rue des Corps-Saints'
        , '1730'
        , 'Geneva'
        , 'Geneve'
        , 'CH'
        );

INSERT INTO locations VALUES 
        ( 3000 
        , 'Murtenstrasse 921'
        , '3095'
        , 'Bern'
        , 'BE'
        , 'CH'
        );

INSERT INTO locations VALUES 
        ( 3100 
        , 'Pieter Breughelstraat 837'
        , '3029SK'
        , 'Utrecht'
        , 'Utrecht'
        , 'NL'
        );

INSERT INTO locations VALUES 
        ( 3200 
        , 'Mariano Escobedo 9991'
        , '11932'
        , 'Mexico City'
        , 'Distrito Federal,'
        , 'MX'
        );


REM ****************************insert data into the DEPARTMENTS table

Prompt ******  Populating DEPARTMENTS table ....

REM disable integrity constraint to EMPLOYEES to load data

ALTER TABLE departments 
  DISABLE CONSTRAINT dept_mgr_fk;

INSERT INTO departments VALUES 
        ( 10
        , 'Administration'
        , 200
        , 1700
        );

INSERT INTO departments VALUES 
        ( 20
        , 'Marketing'
        , 201
        , 1800
        );
                                
INSERT INTO departments VALUES 
        ( 30
        , 'Purchasing'
        , 114
        , 1700
	);
                
INSERT INTO departments VALUES 
        ( 40
        , 'Human Resources'
        , 203
        , 2400
        );

INSERT INTO departments VALUES 
        ( 50
        , 'Shipping'
        , 121
        , 1500
        );
                
INSERT INTO departments VALUES 
        ( 60 
        , 'IT'
        , 103
        , 1400
        );
                
INSERT INTO departments VALUES 
        ( 70 
        , 'Public Relations'
        , 204
        , 2700
        );
                
INSERT INTO departments VALUES 
        ( 80 
        , 'Sales'
        , 145
        , 2500
        );
                
INSERT INTO departments VALUES 
        ( 90 
        , 'Executive'
        , 100
        , 1700
        );

INSERT INTO departments VALUES 
        ( 100 
        , 'Finance'
        , 108
        , 1700
        );
                
INSERT INTO departments VALUES 
        ( 110 
        , 'Accounting'
        , 205
        , 1700
        );

INSERT INTO departments VALUES 
        ( 120 
        , 'Treasury'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 130 
        , 'Corporate Tax'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 140 
        , 'Control And Credit'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 150 
        , 'Shareholder Services'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 160 
        , 'Benefits'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 170 
        , 'Manufacturing'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 180 
        , 'Construction'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 190 
        , 'Contracting'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 200 
        , 'Operations'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 210 
        , 'IT Support'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 220 
        , 'NOC'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 230 
        , 'IT Helpdesk'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 240 
        , 'Government Sales'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 250 
        , 'Retail Sales'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 260 
        , 'Recruiting'
        , NULL
        , 1700
        );

INSERT INTO departments VALUES 
        ( 270 
        , 'Payroll'
        , NULL
        , 1700
        );


REM ***************************insert data into the JOBS table

Prompt ******  Populating JOBS table ....

INSERT INTO jobs VALUES 
        ( 'AD_PRES'
        , 'President'
        , 20080
        , 40000
        );
INSERT INTO jobs VALUES 
        ( 'AD_VP'
        , 'Administration Vice President'
        , 15000
        , 30000
        );

INSERT INTO jobs VALUES 
        ( 'AD_ASST'
        , 'Administration Assistant'
        , 3000
        , 6000
        );

INSERT INTO jobs VALUES 
        ( 'FI_MGR'
        , 'Finance Manager'
        , 8200
        , 16000
        );

INSERT INTO jobs VALUES 
        ( 'FI_ACCOUNT'
        , 'Accountant'
        , 4200
        , 9000
        );

INSERT INTO jobs VALUES 
        ( 'AC_MGR'
        , 'Accounting Manager'
        , 8200
        , 16000
        );

INSERT INTO jobs VALUES 
        ( 'AC_ACCOUNT'
        , 'Public Accountant'
        , 4200
        , 9000
        );
INSERT INTO jobs VALUES 
        ( 'SA_MAN'
        , 'Sales Manager'
        , 10000
        , 20080
        );

INSERT INTO jobs VALUES 
        ( 'SA_REP'
        , 'Sales Representative'
        , 6000
        , 12008
        );

INSERT INTO jobs VALUES 
        ( 'PU_MAN'
        , 'Purchasing Manager'
        , 8000
        , 15000
        );

INSERT INTO jobs VALUES 
        ( 'PU_CLERK'
        , 'Purchasing Clerk'
        , 2500
        , 5500
        );

INSERT INTO jobs VALUES 
        ( 'ST_MAN'
        , 'Stock Manager'
        , 5500
        , 8500
        );
INSERT INTO jobs VALUES 
        ( 'ST_CLERK'
        , 'Stock Clerk'
        , 2008
        , 5000
        );

INSERT INTO jobs VALUES 
        ( 'SH_CLERK'
        , 'Shipping Clerk'
        , 2500
        , 5500
        );

INSERT INTO jobs VALUES 
        ( 'IT_PROG'
        , 'Programmer'
        , 4000
        , 10000
        );

INSERT INTO jobs VALUES 
        ( 'MK_MAN'
        , 'Marketing Manager'
        , 9000
        , 15000
        );

INSERT INTO jobs VALUES 
        ( 'MK_REP'
        , 'Marketing Representative'
        , 4000
        , 9000
        );

INSERT INTO jobs VALUES 
        ( 'HR_REP'
        , 'Human Resources Representative'
        , 4000
        , 9000
        );

INSERT INTO jobs VALUES 
        ( 'PR_REP'
        , 'Public Relations Representative'
        , 4500
        , 10500
        );


REM ***************************insert data into the EMPLOYEES table

Prompt ******  Populating EMPLOYEES table ....

INSERT INTO employees VALUES 
        ( 100
        , 'Steven'
        , 'King'
        , 'SKING'
        , '515.123.4567'
        , TO_DATE('17-06-2003', 'dd-MM-yyyy')
        , 'AD_PRES'
        , 24000
        , NULL
        , NULL
        , 90
        );

INSERT INTO employees VALUES 
        ( 101
        , 'Neena'
        , 'Kochhar'
        , 'NKOCHHAR'
        , '515.123.4568'
        , TO_DATE('21-09-2005', 'dd-MM-yyyy')
        , 'AD_VP'
        , 17000
        , NULL
        , 100
        , 90
        );

INSERT INTO employees VALUES 
        ( 102
        , 'Lex'
        , 'De Haan'
        , 'LDEHAAN'
        , '515.123.4569'
        , TO_DATE('13-01-2001', 'dd-MM-yyyy')
        , 'AD_VP'
        , 17000
        , NULL
        , 100
        , 90
        );

INSERT INTO employees VALUES 
        ( 103
        , 'Alexander'
        , 'Hunold'
        , 'AHUNOLD'
        , '590.423.4567'
        , TO_DATE('03-01-2006', 'dd-MM-yyyy')
        , 'IT_PROG'
        , 9000
        , NULL
        , 102
        , 60
        );

INSERT INTO employees VALUES 
        ( 104
        , 'Bruce'
        , 'Ernst'
        , 'BERNST'
        , '590.423.4568'
        , TO_DATE('21-05-2007', 'dd-MM-yyyy')
        , 'IT_PROG'
        , 6000
        , NULL
        , 103
        , 60
        );

INSERT INTO employees VALUES 
        ( 105
        , 'David'
        , 'Austin'
        , 'DAUSTIN'
        , '590.423.4569'
        , TO_DATE('25-06-2005', 'dd-MM-yyyy')
        , 'IT_PROG'
        , 4800
        , NULL
        , 103
        , 60
        );

INSERT INTO employees VALUES 
        ( 106
        , 'Valli'
        , 'Pataballa'
        , 'VPATABAL'
        , '590.423.4560'
        , TO_DATE('05-02-2006', 'dd-MM-yyyy')
        , 'IT_PROG'
        , 4800
        , NULL
        , 103
        , 60
        );

INSERT INTO employees VALUES 
        ( 107
        , 'Diana'
        , 'Lorentz'
        , 'DLORENTZ'
        , '590.423.5567'
        , TO_DATE('07-02-2007', 'dd-MM-yyyy')
        , 'IT_PROG'
        , 4200
        , NULL
        , 103
        , 60
        );

INSERT INTO employees VALUES 
        ( 108
        , 'Nancy'
        , 'Greenberg'
        , 'NGREENBE'
        , '515.124.4569'
        , TO_DATE('17-08-2002', 'dd-MM-yyyy')
        , 'FI_MGR'
        , 12008
        , NULL
        , 101
        , 100
        );

INSERT INTO employees VALUES 
        ( 109
        , 'Daniel'
        , 'Faviet'
        , 'DFAVIET'
        , '515.124.4169'
        , TO_DATE('16-08-2002', 'dd-MM-yyyy')
        , 'FI_ACCOUNT'
        , 9000
        , NULL
        , 108
        , 100
        );

INSERT INTO employees VALUES 
        ( 110
        , 'John'
        , 'Chen'
        , 'JCHEN'
        , '515.124.4269'
        , TO_DATE('28-09-2005', 'dd-MM-yyyy')
        , 'FI_ACCOUNT'
        , 8200
        , NULL
        , 108
        , 100
        );

INSERT INTO employees VALUES 
        ( 111
        , 'Ismael'
        , 'Sciarra'
        , 'ISCIARRA'
        , '515.124.4369'
        , TO_DATE('30-09-2005', 'dd-MM-yyyy')
        , 'FI_ACCOUNT'
        , 7700
        , NULL
        , 108
        , 100
        );

INSERT INTO employees VALUES 
        ( 112
        , 'Jose Manuel'
        , 'Urman'
        , 'JMURMAN'
        , '515.124.4469'
        , TO_DATE('07-03-2006', 'dd-MM-yyyy')
        , 'FI_ACCOUNT'
        , 7800
        , NULL
        , 108
        , 100
        );

INSERT INTO employees VALUES 
        ( 113
        , 'Luis'
        , 'Popp'
        , 'LPOPP'
        , '515.124.4567'
        , TO_DATE('07-12-2007', 'dd-MM-yyyy')
        , 'FI_ACCOUNT'
        , 6900
        , NULL
        , 108
        , 100
        );

INSERT INTO employees VALUES 
        ( 114
        , 'Den'
        , 'Raphaely'
        , 'DRAPHEAL'
        , '515.127.4561'
        , TO_DATE('07-12-2002', 'dd-MM-yyyy')
        , 'PU_MAN'
        , 11000
        , NULL
        , 100
        , 30
        );

INSERT INTO employees VALUES 
        ( 115
        , 'Alexander'
        , 'Khoo'
        , 'AKHOO'
        , '515.127.4562'
        , TO_DATE('18-05-2003', 'dd-MM-yyyy')
        , 'PU_CLERK'
        , 3100
        , NULL
        , 114
        , 30
        );

INSERT INTO employees VALUES 
        ( 116
        , 'Shelli'
        , 'Baida'
        , 'SBAIDA'
        , '515.127.4563'
        , TO_DATE('24-12-2005', 'dd-MM-yyyy')
        , 'PU_CLERK'
        , 2900
        , NULL
        , 114
        , 30
        );

INSERT INTO employees VALUES 
        ( 117
        , 'Sigal'
        , 'Tobias'
        , 'STOBIAS'
        , '515.127.4564'
        , TO_DATE('24-07-2005', 'dd-MM-yyyy')
        , 'PU_CLERK'
        , 2800
        , NULL
        , 114
        , 30
        );

INSERT INTO employees VALUES 
        ( 118
        , 'Guy'
        , 'Himuro'
        , 'GHIMURO'
        , '515.127.4565'
        , TO_DATE('15-11-2006', 'dd-MM-yyyy')
        , 'PU_CLERK'
        , 2600
        , NULL
        , 114
        , 30
        );

INSERT INTO employees VALUES 
        ( 119
        , 'Karen'
        , 'Colmenares'
        , 'KCOLMENA'
        , '515.127.4566'
        , TO_DATE('10-08-2007', 'dd-MM-yyyy')
        , 'PU_CLERK'
        , 2500
        , NULL
        , 114
        , 30
        );

INSERT INTO employees VALUES 
        ( 120
        , 'Matthew'
        , 'Weiss'
        , 'MWEISS'
        , '650.123.1234'
        , TO_DATE('18-07-2004', 'dd-MM-yyyy')
        , 'ST_MAN'
        , 8000
        , NULL
        , 100
        , 50
        );

INSERT INTO employees VALUES 
        ( 121
        , 'Adam'
        , 'Fripp'
        , 'AFRIPP'
        , '650.123.2234'
        , TO_DATE('10-04-2005', 'dd-MM-yyyy')
        , 'ST_MAN'
        , 8200
        , NULL
        , 100
        , 50
        );

INSERT INTO employees VALUES 
        ( 122
        , 'Payam'
        , 'Kaufling'
        , 'PKAUFLIN'
        , '650.123.3234'
        , TO_DATE('01-05-2003', 'dd-MM-yyyy')
        , 'ST_MAN'
        , 7900
        , NULL
        , 100
        , 50
        );

INSERT INTO employees VALUES 
        ( 123
        , 'Shanta'
        , 'Vollman'
        , 'SVOLLMAN'
        , '650.123.4234'
        , TO_DATE('10-10-2005', 'dd-MM-yyyy')
        , 'ST_MAN'
        , 6500
        , NULL
        , 100
        , 50
        );

INSERT INTO employees VALUES 
        ( 124
        , 'Kevin'
        , 'Mourgos'
        , 'KMOURGOS'
        , '650.123.5234'
        , TO_DATE('16-11-2007', 'dd-MM-yyyy')
        , 'ST_MAN'
        , 5800
        , NULL
        , 100
        , 50
        );

INSERT INTO employees VALUES 
        ( 125
        , 'Julia'
        , 'Nayer'
        , 'JNAYER'
        , '650.124.1214'
        , TO_DATE('16-07-2005', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 3200
        , NULL
        , 120
        , 50
        );

INSERT INTO employees VALUES 
        ( 126
        , 'Irene'
        , 'Mikkilineni'
        , 'IMIKKILI'
        , '650.124.1224'
        , TO_DATE('28-09-2006', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2700
        , NULL
        , 120
        , 50
        );

INSERT INTO employees VALUES 
        ( 127
        , 'James'
        , 'Landry'
        , 'JLANDRY'
        , '650.124.1334'
        , TO_DATE('14-01-2007', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2400
        , NULL
        , 120
        , 50
        );

INSERT INTO employees VALUES 
        ( 128
        , 'Steven'
        , 'Markle'
        , 'SMARKLE'
        , '650.124.1434'
        , TO_DATE('08-03-2008', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2200
        , NULL
        , 120
        , 50
        );

INSERT INTO employees VALUES 
        ( 129
        , 'Laura'
        , 'Bissot'
        , 'LBISSOT'
        , '650.124.5234'
        , TO_DATE('20-08-2005', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 3300
        , NULL
        , 121
        , 50
        );

INSERT INTO employees VALUES 
        ( 130
        , 'Mozhe'
        , 'Atkinson'
        , 'MATKINSO'
        , '650.124.6234'
        , TO_DATE('30-10-2005', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2800
        , NULL
        , 121
        , 50
        );

INSERT INTO employees VALUES 
        ( 131
        , 'James'
        , 'Marlow'
        , 'JAMRLOW'
        , '650.124.7234'
        , TO_DATE('16-02-2005', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2500
        , NULL
        , 121
        , 50
        );

INSERT INTO employees VALUES 
        ( 132
        , 'TJ'
        , 'Olson'
        , 'TJOLSON'
        , '650.124.8234'
        , TO_DATE('10-04-2007', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2100
        , NULL
        , 121
        , 50
        );

INSERT INTO employees VALUES 
        ( 133
        , 'Jason'
        , 'Mallin'
        , 'JMALLIN'
        , '650.127.1934'
        , TO_DATE('14-06-2004', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 3300
        , NULL
        , 122
        , 50
        );

INSERT INTO employees VALUES 
        ( 134
        , 'Michael'
        , 'Rogers'
        , 'MROGERS'
        , '650.127.1834'
        , TO_DATE('26-08-2006', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2900
        , NULL
        , 122
        , 50
        );

INSERT INTO employees VALUES 
        ( 135
        , 'Ki'
        , 'Gee'
        , 'KGEE'
        , '650.127.1734'
        , TO_DATE('12-12-2007', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2400
        , NULL
        , 122
        , 50
        );

INSERT INTO employees VALUES 
        ( 136
        , 'Hazel'
        , 'Philtanker'
        , 'HPHILTAN'
        , '650.127.1634'
        , TO_DATE('06-02-2008', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2200
        , NULL
        , 122
        , 50
        );

INSERT INTO employees VALUES 
        ( 137
        , 'Renske'
        , 'Ladwig'
        , 'RLADWIG'
        , '650.121.1234'
        , TO_DATE('14-07-2003', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 3600
        , NULL
        , 123
        , 50
        );

INSERT INTO employees VALUES 
        ( 138
        , 'Stephen'
        , 'Stiles'
        , 'SSTILES'
        , '650.121.2034'
        , TO_DATE('26-10-2005', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 3200
        , NULL
        , 123
        , 50
        );

INSERT INTO employees VALUES 
        ( 139
        , 'John'
        , 'Seo'
        , 'JSEO'
        , '650.121.2019'
        , TO_DATE('12-02-2006', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2700
        , NULL
        , 123
        , 50
        );

INSERT INTO employees VALUES 
        ( 140
        , 'Joshua'
        , 'Patel'
        , 'JPATEL'
        , '650.121.1834'
        , TO_DATE('06-04-2006', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2500
        , NULL
        , 123
        , 50
        );

INSERT INTO employees VALUES 
        ( 141
        , 'Trenna'
        , 'Rajs'
        , 'TRAJS'
        , '650.121.8009'
        , TO_DATE('17-10-2003', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 3500
        , NULL
        , 124
        , 50
        );

INSERT INTO employees VALUES 
        ( 142
        , 'Curtis'
        , 'Davies'
        , 'CDAVIES'
        , '650.121.2994'
        , TO_DATE('29-01-2005', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 3100
        , NULL
        , 124
        , 50
        );

INSERT INTO employees VALUES 
        ( 143
        , 'Randall'
        , 'Matos'
        , 'RMATOS'
        , '650.121.2874'
        , TO_DATE('15-03-2006', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2600
        , NULL
        , 124
        , 50
        );

INSERT INTO employees VALUES 
        ( 144
        , 'Peter'
        , 'Vargas'
        , 'PVARGAS'
        , '650.121.2004'
        , TO_DATE('09-07-2006', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 2500
        , NULL
        , 124
        , 50
        );

INSERT INTO employees VALUES 
        ( 145
        , 'John'
        , 'Russell'
        , 'JRUSSEL'
        , '011.44.1344.429268'
        , TO_DATE('01-10-2004', 'dd-MM-yyyy')
        , 'SA_MAN'
        , 14000
        , .4
        , 100
        , 80
        );

INSERT INTO employees VALUES 
        ( 146
        , 'Karen'
        , 'Partners'
        , 'KPARTNER'
        , '011.44.1344.467268'
        , TO_DATE('05-01-2005', 'dd-MM-yyyy')
        , 'SA_MAN'
        , 13500
        , .3
        , 100
        , 80
        );

INSERT INTO employees VALUES 
        ( 147
        , 'Alberto'
        , 'Errazuriz'
        , 'AERRAZUR'
        , '011.44.1344.429278'
        , TO_DATE('10-03-2005', 'dd-MM-yyyy')
        , 'SA_MAN'
        , 12000
        , .3
        , 100
        , 80
        );

INSERT INTO employees VALUES 
        ( 148
        , 'Gerald'
        , 'Cambrault'
        , 'GCAMBRAU'
        , '011.44.1344.619268'
        , TO_DATE('15-10-2007', 'dd-MM-yyyy')
        , 'SA_MAN'
        , 11000
        , .3
        , 100
        , 80
        );

INSERT INTO employees VALUES 
        ( 149
        , 'Eleni'
        , 'Zlotkey'
        , 'EZLOTKEY'
        , '011.44.1344.429018'
        , TO_DATE('29-01-2008', 'dd-MM-yyyy')
        , 'SA_MAN'
        , 10500
        , .2
        , 100
        , 80
        );

INSERT INTO employees VALUES 
        ( 150
        , 'Peter'
        , 'Tucker'
        , 'PTUCKER'
        , '011.44.1344.129268'
        , TO_DATE('30-01-2005', 'dd-MM-yyyy')
        , 'SA_REP'
        , 10000
        , .3
        , 145
        , 80
        );

INSERT INTO employees VALUES 
        ( 151
        , 'David'
        , 'Bernstein'
        , 'DBERNSTE'
        , '011.44.1344.345268'
        , TO_DATE('24-03-2005', 'dd-MM-yyyy')
        , 'SA_REP'
        , 9500
        , .25
        , 145
        , 80
        );

INSERT INTO employees VALUES 
        ( 152
        , 'Peter'
        , 'Hall'
        , 'PHALL'
        , '011.44.1344.478968'
        , TO_DATE('20-08-2005', 'dd-MM-yyyy')
        , 'SA_REP'
        , 9000
        , .25
        , 145
        , 80
        );

INSERT INTO employees VALUES 
        ( 153
        , 'Christopher'
        , 'Olsen'
        , 'COLSEN'
        , '011.44.1344.498718'
        , TO_DATE('30-03-2006', 'dd-MM-yyyy')
        , 'SA_REP'
        , 8000
        , .2
        , 145
        , 80
        );

INSERT INTO employees VALUES 
        ( 154
        , 'Nanette'
        , 'Cambrault'
        , 'NCAMBRAU'
        , '011.44.1344.987668'
        , TO_DATE('09-12-2006', 'dd-MM-yyyy')
        , 'SA_REP'
        , 7500
        , .2
        , 145
        , 80
        );

INSERT INTO employees VALUES 
        ( 155
        , 'Oliver'
        , 'Tuvault'
        , 'OTUVAULT'
        , '011.44.1344.486508'
        , TO_DATE('23-11-2007', 'dd-MM-yyyy')
        , 'SA_REP'
        , 7000
        , .15
        , 145
        , 80
        );

INSERT INTO employees VALUES 
        ( 156
        , 'Janette'
        , 'King'
        , 'JKING'
        , '011.44.1345.429268'
        , TO_DATE('30-01-2004', 'dd-MM-yyyy')
        , 'SA_REP'
        , 10000
        , .35
        , 146
        , 80
        );

INSERT INTO employees VALUES 
        ( 157
        , 'Patrick'
        , 'Sully'
        , 'PSULLY'
        , '011.44.1345.929268'
        , TO_DATE('04-03-2004', 'dd-MM-yyyy')
        , 'SA_REP'
        , 9500
        , .35
        , 146
        , 80
        );

INSERT INTO employees VALUES 
        ( 158
        , 'Allan'
        , 'McEwen'
        , 'AMCEWEN'
        , '011.44.1345.829268'
        , TO_DATE('01-08-2004', 'dd-MM-yyyy')
        , 'SA_REP'
        , 9000
        , .35
        , 146
        , 80
        );

INSERT INTO employees VALUES 
        ( 159
        , 'Lindsey'
        , 'Smith'
        , 'LSMITH'
        , '011.44.1345.729268'
        , TO_DATE('10-03-2005', 'dd-MM-yyyy')
        , 'SA_REP'
        , 8000
        , .3
        , 146
        , 80
        );

INSERT INTO employees VALUES 
        ( 160
        , 'Louise'
        , 'Doran'
        , 'LDORAN'
        , '011.44.1345.629268'
        , TO_DATE('15-12-2005', 'dd-MM-yyyy')
        , 'SA_REP'
        , 7500
        , .3
        , 146
        , 80
        );

INSERT INTO employees VALUES 
        ( 161
        , 'Sarath'
        , 'Sewall'
        , 'SSEWALL'
        , '011.44.1345.529268'
        , TO_DATE('03-11-2006', 'dd-MM-yyyy')
        , 'SA_REP'
        , 7000
        , .25
        , 146
        , 80
        );

INSERT INTO employees VALUES 
        ( 162
        , 'Clara'
        , 'Vishney'
        , 'CVISHNEY'
        , '011.44.1346.129268'
        , TO_DATE('11-11-2005', 'dd-MM-yyyy')
        , 'SA_REP'
        , 10500
        , .25
        , 147
        , 80
        );

INSERT INTO employees VALUES 
        ( 163
        , 'Danielle'
        , 'Greene'
        , 'DGREENE'
        , '011.44.1346.229268'
        , TO_DATE('19-03-2007', 'dd-MM-yyyy')
        , 'SA_REP'
        , 9500
        , .15
        , 147
        , 80
        );

INSERT INTO employees VALUES 
        ( 164
        , 'Mattea'
        , 'Marvins'
        , 'MMARVINS'
        , '011.44.1346.329268'
        , TO_DATE('24-01-2008', 'dd-MM-yyyy')
        , 'SA_REP'
        , 7200
        , .10
        , 147
        , 80
        );

INSERT INTO employees VALUES 
        ( 165
        , 'David'
        , 'Lee'
        , 'DLEE'
        , '011.44.1346.529268'
        , TO_DATE('23-02-2008', 'dd-MM-yyyy')
        , 'SA_REP'
        , 6800
        , .1
        , 147
        , 80
        );

INSERT INTO employees VALUES 
        ( 166
        , 'Sundar'
        , 'Ande'
        , 'SANDE'
        , '011.44.1346.629268'
        , TO_DATE('24-03-2008', 'dd-MM-yyyy')
        , 'SA_REP'
        , 6400
        , .10
        , 147
        , 80
        );

INSERT INTO employees VALUES 
        ( 167
        , 'Amit'
        , 'Banda'
        , 'ABANDA'
        , '011.44.1346.729268'
        , TO_DATE('21-04-2008', 'dd-MM-yyyy')
        , 'SA_REP'
        , 6200
        , .10
        , 147
        , 80
        );

INSERT INTO employees VALUES 
        ( 168
        , 'Lisa'
        , 'Ozer'
        , 'LOZER'
        , '011.44.1343.929268'
        , TO_DATE('11-03-2005', 'dd-MM-yyyy')
        , 'SA_REP'
        , 11500
        , .25
        , 148
        , 80
        );

INSERT INTO employees VALUES 
        ( 169  
        , 'Harrison'
        , 'Bloom'
        , 'HBLOOM'
        , '011.44.1343.829268'
        , TO_DATE('23-03-2006', 'dd-MM-yyyy')
        , 'SA_REP'
        , 10000
        , .20
        , 148
        , 80
        );

INSERT INTO employees VALUES 
        ( 170
        , 'Tayler'
        , 'Fox'
        , 'TFOX'
        , '011.44.1343.729268'
        , TO_DATE('24-01-2006', 'dd-MM-yyyy')
        , 'SA_REP'
        , 9600
        , .20
        , 148
        , 80
        );

INSERT INTO employees VALUES 
        ( 171
        , 'William'
        , 'Smith'
        , 'WSMITH'
        , '011.44.1343.629268'
        , TO_DATE('23-02-2007', 'dd-MM-yyyy')
        , 'SA_REP'
        , 7400
        , .15
        , 148
        , 80
        );

INSERT INTO employees VALUES 
        ( 172
        , 'Elizabeth'
        , 'Bates'
        , 'EBATES'
        , '011.44.1343.529268'
        , TO_DATE('24-03-2007', 'dd-MM-yyyy')
        , 'SA_REP'
        , 7300
        , .15
        , 148
        , 80
        );

INSERT INTO employees VALUES 
        ( 173
        , 'Sundita'
        , 'Kumar'
        , 'SKUMAR'
        , '011.44.1343.329268'
        , TO_DATE('21-04-2008', 'dd-MM-yyyy')
        , 'SA_REP'
        , 6100
        , .10
        , 148
        , 80
        );

INSERT INTO employees VALUES 
        ( 174
        , 'Ellen'
        , 'Abel'
        , 'EABEL'
        , '011.44.1644.429267'
        , TO_DATE('11-05-2004', 'dd-MM-yyyy')
        , 'SA_REP'
        , 11000
        , .30
        , 149
        , 80
        );

INSERT INTO employees VALUES 
        ( 175
        , 'Alyssa'
        , 'Hutton'
        , 'AHUTTON'
        , '011.44.1644.429266'
        , TO_DATE('19-03-2005', 'dd-MM-yyyy')
        , 'SA_REP'
        , 8800
        , .25
        , 149
        , 80
        );

INSERT INTO employees VALUES 
        ( 176
        , 'Jonathon'
        , 'Taylor'
        , 'JTAYLOR'
        , '011.44.1644.429265'
        , TO_DATE('24-03-2006', 'dd-MM-yyyy')
        , 'SA_REP'
        , 8600
        , .20
        , 149
        , 80
        );

INSERT INTO employees VALUES 
        ( 177
        , 'Jack'
        , 'Livingston'
        , 'JLIVINGS'
        , '011.44.1644.429264'
        , TO_DATE('23-04-2006', 'dd-MM-yyyy')
        , 'SA_REP'
        , 8400
        , .20
        , 149
        , 80
        );

INSERT INTO employees VALUES 
        ( 178
        , 'Kimberely'
        , 'Grant'
        , 'KGRANT'
        , '011.44.1644.429263'
        , TO_DATE('24-05-2007', 'dd-MM-yyyy')
        , 'SA_REP'
        , 7000
        , .15
        , 149
        , NULL
        );

INSERT INTO employees VALUES 
        ( 179
        , 'Charles'
        , 'Johnson'
        , 'CJOHNSON'
        , '011.44.1644.429262'
        , TO_DATE('04-01-2008', 'dd-MM-yyyy')
        , 'SA_REP'
        , 6200
        , .10
        , 149
        , 80
        );

INSERT INTO employees VALUES 
        ( 180
        , 'Winston'
        , 'Taylor'
        , 'WTAYLOR'
        , '650.507.9876'
        , TO_DATE('24-01-2006', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3200
        , NULL
        , 120
        , 50
        );

INSERT INTO employees VALUES 
        ( 181
        , 'Jean'
        , 'Fleaur'
        , 'JFLEAUR'
        , '650.507.9877'
        , TO_DATE('23-02-2006', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3100
        , NULL
        , 120
        , 50
        );

INSERT INTO employees VALUES 
        ( 182
        , 'Martha'
        , 'Sullivan'
        , 'MSULLIVA'
        , '650.507.9878'
        , TO_DATE('21-06-2007', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 2500
        , NULL
        , 120
        , 50
        );

INSERT INTO employees VALUES 
        ( 183
        , 'Girard'
        , 'Geoni'
        , 'GGEONI'
        , '650.507.9879'
        , TO_DATE('03-02-2008', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 2800
        , NULL
        , 120
        , 50
        );

INSERT INTO employees VALUES 
        ( 184
        , 'Nandita'
        , 'Sarchand'
        , 'NSARCHAN'
        , '650.509.1876'
        , TO_DATE('27-01-2004', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 4200
        , NULL
        , 121
        , 50
        );

INSERT INTO employees VALUES 
        ( 185
        , 'Alexis'
        , 'Bull'
        , 'ABULL'
        , '650.509.2876'
        , TO_DATE('20-02-2005', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 4100
        , NULL
        , 121
        , 50
        );

INSERT INTO employees VALUES 
        ( 186
        , 'Julia'
        , 'Dellinger'
        , 'JDELLING'
        , '650.509.3876'
        , TO_DATE('24-06-2006', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3400
        , NULL
        , 121
        , 50
        );

INSERT INTO employees VALUES 
        ( 187
        , 'Anthony'
        , 'Cabrio'
        , 'ACABRIO'
        , '650.509.4876'
        , TO_DATE('07-02-2007', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3000
        , NULL
        , 121
        , 50
        );

INSERT INTO employees VALUES 
        ( 188
        , 'Kelly'
        , 'Chung'
        , 'KCHUNG'
        , '650.505.1876'
        , TO_DATE('14-06-2005', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3800
        , NULL
        , 122
        , 50
        );

INSERT INTO employees VALUES 
        ( 189
        , 'Jennifer'
        , 'Dilly'
        , 'JDILLY'
        , '650.505.2876'
        , TO_DATE('13-08-2005', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3600
        , NULL
        , 122
        , 50
        );

INSERT INTO employees VALUES 
        ( 190
        , 'Timothy'
        , 'Gates'
        , 'TGATES'
        , '650.505.3876'
        , TO_DATE('11-07-2006', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 2900
        , NULL
        , 122
        , 50
        );

INSERT INTO employees VALUES 
        ( 191
        , 'Randall'
        , 'Perkins'
        , 'RPERKINS'
        , '650.505.4876'
        , TO_DATE('19-12-2007', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 2500
        , NULL
        , 122
        , 50
        );

INSERT INTO employees VALUES 
        ( 192
        , 'Sarah'
        , 'Bell'
        , 'SBELL'
        , '650.501.1876'
        , TO_DATE('04-02-2004', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 4000
        , NULL
        , 123
        , 50
        );

INSERT INTO employees VALUES 
        ( 193
        , 'Britney'
        , 'Everett'
        , 'BEVERETT'
        , '650.501.2876'
        , TO_DATE('03-03-2005', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3900
        , NULL
        , 123
        , 50
        );

INSERT INTO employees VALUES 
        ( 194
        , 'Samuel'
        , 'McCain'
        , 'SMCCAIN'
        , '650.501.3876'
        , TO_DATE('01-07-2006', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3200
        , NULL
        , 123
        , 50
        );

INSERT INTO employees VALUES 
        ( 195
        , 'Vance'
        , 'Jones'
        , 'VJONES'
        , '650.501.4876'
        , TO_DATE('17-03-2007', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 2800
        , NULL
        , 123
        , 50
        );

INSERT INTO employees VALUES 
        ( 196
        , 'Alana'
        , 'Walsh'
        , 'AWALSH'
        , '650.507.9811'
        , TO_DATE('24-04-2006', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3100
        , NULL
        , 124
        , 50
        );

INSERT INTO employees VALUES 
        ( 197
        , 'Kevin'
        , 'Feeney'
        , 'KFEENEY'
        , '650.507.9822'
        , TO_DATE('23-05-2006', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 3000
        , NULL
        , 124
        , 50
        );

INSERT INTO employees VALUES 
        ( 198
        , 'Donald'
        , 'OConnell'
        , 'DOCONNEL'
        , '650.507.9833'
        , TO_DATE('21-06-2007', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 2600
        , NULL
        , 124
        , 50
        );

INSERT INTO employees VALUES 
        ( 199
        , 'Douglas'
        , 'Grant'
        , 'DGRANT'
        , '650.507.9844'
        , TO_DATE('13-01-2008', 'dd-MM-yyyy')
        , 'SH_CLERK'
        , 2600
        , NULL
        , 124
        , 50
        );

INSERT INTO employees VALUES 
        ( 200
        , 'Jennifer'
        , 'Whalen'
        , 'JWHALEN'
        , '515.123.4444'
        , TO_DATE('17-09-2003', 'dd-MM-yyyy')
        , 'AD_ASST'
        , 4400
        , NULL
        , 101
        , 10
        );

INSERT INTO employees VALUES 
        ( 201
        , 'Michael'
        , 'Hartstein'
        , 'MHARTSTE'
        , '515.123.5555'
        , TO_DATE('17-02-2004', 'dd-MM-yyyy')
        , 'MK_MAN'
        , 13000
        , NULL
        , 100
        , 20
        );

INSERT INTO employees VALUES 
        ( 202
        , 'Pat'
        , 'Fay'
        , 'PFAY'
        , '603.123.6666'
        , TO_DATE('17-08-2005', 'dd-MM-yyyy')
        , 'MK_REP'
        , 6000
        , NULL
        , 201
        , 20
        );

INSERT INTO employees VALUES 
        ( 203
        , 'Susan'
        , 'Mavris'
        , 'SMAVRIS'
        , '515.123.7777'
        , TO_DATE('07-06-2002', 'dd-MM-yyyy')
        , 'HR_REP'
        , 6500
        , NULL
        , 101
        , 40
        );

INSERT INTO employees VALUES 
        ( 204
        , 'Hermann'
        , 'Baer'
        , 'HBAER'
        , '515.123.8888'
        , TO_DATE('07-06-2002', 'dd-MM-yyyy')
        , 'PR_REP'
        , 10000
        , NULL
        , 101
        , 70
        );

INSERT INTO employees VALUES 
        ( 205
        , 'Shelley'
        , 'Higgins'
        , 'SHIGGINS'
        , '515.123.8080'
        , TO_DATE('07-06-2002', 'dd-MM-yyyy')
        , 'AC_MGR'
        , 12008
        , NULL
        , 101
        , 110
        );

INSERT INTO employees VALUES 
        ( 206
        , 'William'
        , 'Gietz'
        , 'WGIETZ'
        , '515.123.8181'
        , TO_DATE('07-06-2002', 'dd-MM-yyyy')
        , 'AC_ACCOUNT'
        , 8300
        , NULL
        , 205
        , 110
        );

REM ********* insert data into the JOB_HISTORY table

Prompt ******  Populating JOB_HISTORY table ....


INSERT INTO job_history
VALUES (102
       , TO_DATE('13-01-2001', 'dd-MM-yyyy')
       , TO_DATE('24-07-2006', 'dd-MM-yyyy')
       , 'IT_PROG'
       , 60);

INSERT INTO job_history
VALUES (101
       , TO_DATE('21-09-1997', 'dd-MM-yyyy')
       , TO_DATE('27-10-2001', 'dd-MM-yyyy')
       , 'AC_ACCOUNT'
       , 110);

INSERT INTO job_history
VALUES (101
       , TO_DATE('28-10-2001', 'dd-MM-yyyy')
       , TO_DATE('15-03-2005', 'dd-MM-yyyy')
       , 'AC_MGR'
       , 110);

INSERT INTO job_history
VALUES (201
       , TO_DATE('17-02-2004', 'dd-MM-yyyy')
       , TO_DATE('19-12-2007', 'dd-MM-yyyy')
       , 'MK_REP'
       , 20);

INSERT INTO job_history
VALUES  (114
        , TO_DATE('24-03-2006', 'dd-MM-yyyy')
        , TO_DATE('31-12-2007', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 50
        );

INSERT INTO job_history
VALUES  (122
        , TO_DATE('01-01-2007', 'dd-MM-yyyy')
        , TO_DATE('31-12-2007', 'dd-MM-yyyy')
        , 'ST_CLERK'
        , 50
        );

INSERT INTO job_history
VALUES  (200
        , TO_DATE('17-09-1995', 'dd-MM-yyyy')
        , TO_DATE('17-06-2001', 'dd-MM-yyyy')
        , 'AD_ASST'
        , 90
        );

INSERT INTO job_history
VALUES  (176
        , TO_DATE('24-03-2006', 'dd-MM-yyyy')
        , TO_DATE('31-12-2006', 'dd-MM-yyyy')
        , 'SA_REP'
        , 80
        );

INSERT INTO job_history
VALUES  (176
        , TO_DATE('01-01-2007', 'dd-MM-yyyy')
        , TO_DATE('31-12-2007', 'dd-MM-yyyy')
        , 'SA_MAN'
        , 80
        );

INSERT INTO job_history
VALUES  (200
        , TO_DATE('01-07-2002', 'dd-MM-yyyy')
        , TO_DATE('31-12-2006', 'dd-MM-yyyy')
        , 'AC_ACCOUNT'
        , 90
        );

REM enable integrity constraint to DEPARTMENTS

ALTER TABLE departments 
  ENABLE CONSTRAINT dept_mgr_fk;

COMMIT;

--------------------------
-- CREATING OTHER USERS --
--------------------------

CONNECT system/&pass;

DROP USER APP_DATA CASCADE;

CREATE USER APP_DATA IDENTIFIED BY &pass
DEFAULT TABLESPACE USERS
QUOTA UNLIMITED ON USERS;

DROP USER APP_CODE CASCADE;

CREATE USER APP_CODE IDENTIFIED BY &pass
DEFAULT TABLESPACE USERS
QUOTA UNLIMITED ON USERS;

DROP USER APP_ADMIN CASCADE;

CREATE USER APP_ADMIN IDENTIFIED BY &pass
DEFAULT TABLESPACE USERS
QUOTA UNLIMITED ON USERS;

DROP USER APP_USER CASCADE;

CREATE USER APP_USER IDENTIFIED BY &pass
DEFAULT TABLESPACE USERS
QUOTA UNLIMITED ON USERS;

DROP USER APP_ADMIN_USER CASCADE;

CREATE USER APP_ADMIN_USER IDENTIFIED BY &pass
DEFAULT TABLESPACE USERS
QUOTA UNLIMITED ON USERS;

COMMIT;

---------------------
-- GRANTING RIGHTS --
---------------------

CONNECT system/&pass;

GRANT CREATE SESSION TO APP_DATA;
GRANT CREATE TABLE, CREATE VIEW, CREATE TRIGGER, CREATE SEQUENCE TO APP_DATA;
GRANT SELECT ON HR.DEPARTMENTS TO APP_DATA;
GRANT SELECT ON HR.EMPLOYEES TO APP_DATA;
GRANT SELECT ON HR.JOB_HISTORY TO APP_DATA;
GRANT SELECT ON HR.JOBS TO APP_DATA;

GRANT CREATE SESSION TO APP_CODE;
GRANT CREATE PROCEDURE TO APP_CODE;
GRANT CREATE SYNONYM TO APP_CODE;

GRANT CREATE SESSION TO APP_ADMIN;
GRANT CREATE PROCEDURE TO APP_ADMIN;
GRANT CREATE SYNONYM TO APP_ADMIN;

GRANT CREATE SESSION TO APP_USER;
GRANT CREATE SYNONYM TO APP_USER;

GRANT CREATE SESSION TO APP_ADMIN_USER;
GRANT CREATE SYNONYM TO APP_ADMIN_USER;

COMMIT;

-------------------------------
-- CREATING APP_DATA OBJECTS --
-------------------------------

CONNECT app_data/&pass

--DROP TABLE employees#;

--DROP VIEW employees;

CREATE TABLE employees#
(
employee_id NUMBER(6) CONSTRAINT employees#_pk PRIMARY KEY,
first_name VARCHAR2(20) CONSTRAINT emp#_first_name_not_null NOT NULL,
last_name VARCHAR2(25) CONSTRAINT emp#_last_name_not_null NOT NULL,
email_addr VARCHAR2(25) CONSTRAINT emp#_email_addr_not_null NOT NULL,
hire_date DATE DEFAULT TRUNC(SYSDATE) CONSTRAINT emp#_hire_date_not_null NOT NULL,
country_code VARCHAR2(5) CONSTRAINT emp#_country_code_not_null NOT NULL,
phone_number VARCHAR2(20) CONSTRAINT emp#_phone_number_not_null NOT NULL,
job_id VARCHAR2(10) CONSTRAINT emp#_job_id_not_null NOT NULL,
job_start_date DATE CONSTRAINT emp#_job_start_date_not_null NOT NULL,
salary NUMBER(6) CONSTRAINT emp#_salary_not_null NOT NULL,
manager_id NUMBER (6),
department_id NUMBER (4)
);

CREATE VIEW employees AS SELECT * FROM employees#;

--DROP TABLE jobs#;

--DROP VIEW jobs;

CREATE TABLE jobs#
(
job_id VARCHAR2(10) CONSTRAINT jobs#_pk PRIMARY KEY,
job_title VARCHAR2(35) CONSTRAINT jobs#_title_not_null NOT NULL,
min_salary NUMBER(6) CONSTRAINT jobs#_min_salary_not_null NOT NULL,
max_salary NUMBER(6) CONSTRAINT jobs#_max_salary_not_null NOT NULL
);

CREATE VIEW jobs AS SELECT * FROM jobs#;

--DROP TABLE departments#;

--DROP VIEW departments;

CREATE TABLE departments#
(
department_id NUMBER(4) CONSTRAINT departments#_pk PRIMARY KEY,
department_name VARCHAR2(30) CONSTRAINT dep#_name_not_null NOT NULL
	CONSTRAINT dep#_name_unique UNIQUE,
manager_id NUMBER(6)
);

CREATE VIEW departments AS SELECT * FROM departments#;

--DROP TABLE job_history#;

--DROP VIEW job_history;

CREATE TABLE job_history#
(
employee_id NUMBER(6),
job_id VARCHAR2(10),
start_date DATE CONSTRAINT jh#_start_date_not_null NOT NULL,
end_date DATE CONSTRAINT jh#_end_date_not_null NOT NULL,
department_id NUMBER(4) CONSTRAINT jh#_department_id_not_null NOT NULL,
CONSTRAINT job_history#_pk PRIMARY KEY (employee_id, start_date)
);

CREATE VIEW job_history AS SELECT * FROM job_history#;

COMMIT;

------------------------------
-- FILLING APP_DATA OBJECTS --
------------------------------

CONNECT app_data/&pass

INSERT INTO employees (employee_id, first_name, last_name, email_addr,
 hire_date, country_code, phone_number, job_id, job_start_date, salary,
 manager_id, department_id)
SELECT employee_id, first_name, last_name, email, hire_date,
 CASE WHEN phone_number LIKE '011.%'
 THEN '+' || SUBSTR( phone_number, INSTR( phone_number, '.' )+1,
 INSTR( phone_number, '.', 1, 2 ) - INSTR( phone_number, '.' ) - 1 )
 ELSE '+1'
END country_code,
CASE WHEN phone_number LIKE '011.%'
 THEN SUBSTR( phone_number, INSTR(phone_number, '.', 1, 2 )+1 )
 ELSE phone_number
END phone_number,
job_id,
NVL( (SELECT MAX(end_date+1)
 FROM HR.JOB_HISTORY jh
 WHERE jh.employee_id = employees.employee_id), hire_date),
salary, manager_id, department_id
FROM HR.EMPLOYEES;

INSERT INTO jobs SELECT * FROM hr.jobs;

INSERT INTO departments
SELECT department_id, department_name, manager_id
FROM hr.departments;

INSERT INTO job_history
SELECT employee_id, job_id, start_date, end_date, department_id
FROM hr.job_history;

COMMIT;

---------------------------
-- REMAINING CONSTRAINTS --
---------------------------

CONNECT app_data/&pass

--ALTER TABLE employees#
--DROP CONSTRAINT emp#_job_id_fk;

ALTER TABLE employees#
ADD CONSTRAINT emp#_job_id_fk
FOREIGN KEY (job_id)
REFERENCES jobs# (job_id);

--ALTER TABLE employees#
--DROP CONSTRAINT emp#_manager_id_fk;

ALTER TABLE employees#
ADD CONSTRAINT emp#_manager_id_fk
FOREIGN KEY (manager_id)
REFERENCES employees# (employee_id);

--ALTER TABLE employees#
--DROP CONSTRAINT emp#_department_id_fk;

ALTER TABLE employees#
ADD CONSTRAINT emp#_department_id_fk
FOREIGN KEY (department_id)
REFERENCES departments# (department_id);

ALTER TABLE employees#
ADD CONSTRAINT emp#_hire_date_check
CHECK (TRUNC(hire_date) = hire_date);

ALTER TABLE employees#
ADD CONSTRAINT emp#_job_start_date_check
CHECK (TRUNC(job_start_date) = job_start_date);

--ALTER TABLE departments#
--DROP CONSTRAINT dep#_manager_id_fk;

ALTER TABLE departments#
ADD CONSTRAINT dep#_manager_id_fk
FOREIGN KEY (manager_id)
REFERENCES employees# (employee_id);

--ALTER TABLE job_history#
--DROP CONSTRAINT jh#_employee_id_fk;

ALTER TABLE job_history#
ADD CONSTRAINT jh#_employee_id_fk
FOREIGN KEY (employee_id)
REFERENCES employees# (employee_id);

--ALTER TABLE job_history#
--DROP CONSTRAINT jh#_job_id_fk;

ALTER TABLE job_history#
ADD CONSTRAINT jh#_job_id_fk
FOREIGN KEY (job_id)
REFERENCES jobs# (job_id);

--ALTER TABLE job_history#
--DROP CONSTRAINT jh#_department_id_fk;

ALTER TABLE job_history#
ADD CONSTRAINT jh#_department_id_fk
FOREIGN KEY (department_id)
REFERENCES departments# (department_id);

ALTER TABLE job_history#
ADD CONSTRAINT jh#_date_check
CHECK (start_date < end_date);

COMMIT;

---------------
-- SEQUENCES --
---------------

CONNECT app_data/&pass

CREATE SEQUENCE employees#_sequence INCREMENT BY 1 START
WITH 207;
CREATE SEQUENCE departments#_sequence INCREMENT BY 10 START
WITH 280;

COMMIT;

--------------
-- TRIGGERS --
--------------

CONNECT app_data/&pass

CREATE OR REPLACE TRIGGER employees#_aiufer
AFTER INSERT OR UPDATE OF salary, job_id ON employees# FOR EACH ROW
DECLARE
 l_cnt NUMBER;
BEGIN
LOCK TABLE jobs IN SHARE MODE; -- Ensure that jobs does not change
                               -- during the following query.
     SELECT COUNT(*) INTO l_cnt
     FROM jobs
     WHERE job_id = :NEW.job_id
     AND :NEW.salary BETWEEN min_salary AND max_salary;

     IF (l_cnt<>1) THEN
       RAISE_APPLICATION_ERROR( -20002,
         CASE
           WHEN :new.job_id = :old.job_id
           THEN 'Salary modification invalid'
           ELSE 'Job reassignment puts salary out of range'
         END );
     END IF;
END;
/

CREATE OR REPLACE TRIGGER jobs#_aufer
AFTER UPDATE OF min_salary, max_salary ON jobs# FOR EACH ROW
WHEN (NEW.min_salary > OLD.min_salary OR NEW.max_salary < OLD.max_salary)
DECLARE
 l_cnt NUMBER;
BEGIN
 LOCK TABLE employees# IN SHARE MODE;
 SELECT COUNT(*) INTO l_cnt
 FROM employees#
 WHERE job_id = :NEW.job_id
 AND salary NOT BETWEEN :NEW.min_salary and :NEW.max_salary;

 IF (l_cnt>0) THEN
 RAISE_APPLICATION_ERROR( -20001,
 'Salary update would violate ' || l_cnt || ' existing employee records' );
 END IF;
END;
/

COMMIT;

--------------------
-- OBJECT RECHTEN --
--------------------

CONNECT app_data/&pass

GRANT SELECT ON EMPLOYEES TO APP_CODE;
GRANT INSERT ON EMPLOYEES TO APP_CODE;
GRANT UPDATE ON EMPLOYEES TO APP_CODE;
GRANT DELETE ON EMPLOYEES TO APP_CODE;
GRANT SELECT ON DEPARTMENTS TO APP_CODE;
GRANT SELECT ON JOBS TO APP_CODE;
GRANT SELECT ON JOB_HISTORY TO APP_CODE;
GRANT INSERT ON JOB_HISTORY TO APP_CODE;
GRANT SELECT ON EMPLOYEES#_SEQUENCE TO APP_CODE;

GRANT SELECT ON JOBS TO APP_ADMIN;
GRANT INSERT ON JOBS TO APP_ADMIN;
GRANT UPDATE ON JOBS TO APP_ADMIN;
GRANT DELETE ON JOBS TO APP_ADMIN;
GRANT SELECT ON DEPARTMENTS TO APP_ADMIN;
GRANT INSERT ON DEPARTMENTS TO APP_ADMIN;
GRANT UPDATE ON DEPARTMENTS TO APP_ADMIN;
GRANT DELETE ON DEPARTMENTS TO APP_ADMIN;
GRANT SELECT ON EMPLOYEES#_SEQUENCE TO APP_ADMIN;
GRANT SELECT ON DEPARTMENTS#_SEQUENCE TO APP_ADMIN;

GRANT SELECT ON JOBS TO APP_ADMIN_USER;
GRANT SELECT ON DEPARTMENTS TO APP_ADMIN_USER;

COMMIT;

--------------
-- SYNONYMS --
--------------

CONNECT app_code/&pass

CREATE OR REPLACE SYNONYM employees
FOR app_data.employees;

CREATE OR REPLACE SYNONYM departments
FOR app_data.departments;

CREATE OR REPLACE SYNONYM jobs
FOR app_data.jobs;

CREATE OR REPLACE SYNONYM job_history
FOR app_data.job_history;

CONNECT app_admin/&pass

CREATE OR REPLACE SYNONYM departments
FOR app_data.departments;

CREATE OR REPLACE SYNONYM jobs
FOR app_data.jobs;

CREATE OR REPLACE SYNONYM departments_sequence
FOR app_data.departments#_sequence;

COMMIT;

--------------
-- PACKAGES --
--------------

CONNECT app_code/&pass

CREATE OR REPLACE PACKAGE employees_pkg
AS
  PROCEDURE get_employees_in_dept
    ( p_deptno IN employees.department_id%TYPE,
      p_result_set IN OUT SYS_REFCURSOR );
  PROCEDURE get_job_history
    ( p_employee_id IN employees.department_id%TYPE,
      p_result_set IN OUT SYS_REFCURSOR );
  PROCEDURE show_employee
    ( p_employee_id IN employees.employee_id%TYPE,
      p_result_set IN OUT SYS_REFCURSOR );
  PROCEDURE update_salary
    ( p_employee_id IN employees.employee_id%TYPE,
      p_new_salary IN employees.salary%TYPE );
  PROCEDURE change_job
    ( p_employee_id IN employees.employee_id%TYPE,
      p_new_job IN employees.job_id%TYPE,
      p_new_salary IN employees.salary%TYPE := NULL,
      p_new_dept IN employees.department_id%TYPE := NULL );
END employees_pkg;
/

CREATE OR REPLACE PACKAGE BODY employees_pkg
AS
  PROCEDURE get_employees_in_dept
    ( p_deptno IN employees.department_id%TYPE,
      p_result_set IN OUT SYS_REFCURSOR )
  IS
    l_cursor SYS_REFCURSOR;
  BEGIN
    OPEN p_result_set FOR
      SELECT e.employee_id,
        e.first_name || ' ' || e.last_name name,
        TO_CHAR( e.hire_date, 'Dy Mon ddth, yyyy' ) hire_date,
        j.job_title,
        m.first_name || ' ' || m.last_name manager,
        d.department_name
      FROM employees e INNER JOIN jobs j ON (e.job_id = j.job_id)
        LEFT OUTER JOIN employees m ON (e.manager_id = m.employee_id)
        INNER JOIN departments d ON (e.department_id = d.department_id)
      WHERE e.department_id = p_deptno ;
  END get_employees_in_dept;

  PROCEDURE get_job_history
    ( p_employee_id IN employees.department_id%TYPE,
      p_result_set IN OUT SYS_REFCURSOR )
  IS
  BEGIN
    OPEN p_result_set FOR
      SELECT e.First_name || ' ' || e.last_name name, j.job_title,
        e.job_start_date start_date,
        TO_DATE(NULL) end_date
      FROM employees e INNER JOIN jobs j ON (e.job_id = j.job_id)
      WHERE e.employee_id = p_employee_id
      UNION ALL
      SELECT e.First_name || ' ' || e.last_name name,
        j.job_title,
        jh.start_date,
        jh.end_date
      FROM employees e INNER JOIN job_history jh
        ON (e.employee_id = jh.employee_id)
        INNER JOIN jobs j ON (jh.job_id = j.job_id)
      WHERE e.employee_id = p_employee_id
      ORDER BY start_date DESC;
  END get_job_history;

  PROCEDURE show_employee
    ( p_employee_id IN employees.employee_id%TYPE,
      p_result_set IN OUT sys_refcursor )
  IS
  BEGIN
    OPEN p_result_set FOR
      SELECT *
      FROM (SELECT TO_CHAR(e.employee_id) employee_id,
              e.first_name || ' ' || e.last_name name,
              e.email_addr,
              TO_CHAR(e.hire_date,'dd-mon-yyyy') hire_date,
              e.country_code,
              e.phone_number,
              j.job_title,
              TO_CHAR(e.job_start_date,'dd-mon-yyyy') job_start_date,
              to_char(e.salary) salary,
              m.first_name || ' ' || m.last_name manager,
              d.department_name
            FROM employees e INNER JOIN jobs j on (e.job_id = j.job_id)
              RIGHT OUTER JOIN employees m ON (m.employee_id = e.manager_id)
              INNER JOIN departments d ON (e.department_id = d.department_id)
            WHERE e.employee_id = p_employee_id)
      UNPIVOT (VALUE FOR ATTRIBUTE IN (employee_id, name, email_addr, hire_date,
        country_code, phone_number, job_title, job_start_date, salary, manager,
        department_name) );
  END show_employee;

  PROCEDURE update_salary
    ( p_employee_id IN employees.employee_id%type,
      p_new_salary IN employees.salary%type )
  IS
  BEGIN
    UPDATE employees
    SET salary = p_new_salary
    WHERE employee_id = p_employee_id;
  END update_salary;

  PROCEDURE change_job
    ( p_employee_id IN employees.employee_id%TYPE,
      p_new_job IN employees.job_id%TYPE,
      p_new_salary IN employees.salary%TYPE := NULL,
      p_new_dept IN employees.department_id%TYPE := NULL )
  IS
  BEGIN
    INSERT INTO job_history (employee_id, start_date, end_date, job_id,
    department_id)
    SELECT employee_id, job_start_date, TRUNC(SYSDATE), job_id, department_id
    FROM employees
    WHERE employee_id = p_employee_id;
    UPDATE employees
    SET job_id = p_new_job,
    department_id = NVL( p_new_dept, department_id ),
    salary = NVL( p_new_salary, salary ),
    job_start_date = TRUNC(SYSDATE)
    WHERE employee_id = p_employee_id;
  END change_job;
END employees_pkg;
/

CONNECT app_admin/&pass

CREATE OR REPLACE PACKAGE admin_pkg
AS
  PROCEDURE update_job
    ( p_job_id IN jobs.job_id%TYPE,
      p_job_title IN jobs.job_title%TYPE := NULL,
      p_min_salary IN jobs.min_salary%TYPE := NULL,
      p_max_salary IN jobs.max_salary%TYPE := NULL );
  PROCEDURE add_job
    ( p_job_id IN jobs.job_id%TYPE,
      p_job_title IN jobs.job_title%TYPE,
      p_min_salary IN jobs.min_salary%TYPE,
      p_max_salary IN jobs.max_salary%TYPE );
  PROCEDURE update_department
    ( p_department_id IN departments.department_id%TYPE,
      p_department_name IN departments.department_name%TYPE := NULL,
      p_manager_id IN departments.manager_id%TYPE := NULL,
      p_update_manager_id IN BOOLEAN := FALSE );
  FUNCTION add_department
    ( p_department_name IN departments.department_name%TYPE,
      p_manager_id IN departments.manager_id%TYPE )
    RETURN departments.department_id%TYPE;
END admin_pkg;
/

CREATE OR REPLACE PACKAGE BODY admin_pkg
AS
  PROCEDURE update_job
    ( p_job_id IN jobs.job_id%TYPE,
      p_job_title IN jobs.job_title%TYPE := NULL,
      p_min_salary IN jobs.min_salary%TYPE := NULL,
      p_max_salary IN jobs.max_salary%TYPE := NULL )
  IS
  BEGIN
    UPDATE jobs
    SET job_title = NVL( p_job_title, job_title ),
    min_salary = NVL( p_min_salary, min_salary ),
    max_salary = NVL( p_max_salary, max_salary )
    WHERE job_id = p_job_id;
  END update_job;

  PROCEDURE add_job
    ( p_job_id IN jobs.job_id%TYPE,
      p_job_title IN jobs.job_title%TYPE,
      p_min_salary IN jobs.min_salary%TYPE,
      p_max_salary IN jobs.max_salary%TYPE )
  IS
  BEGIN
  INSERT INTO jobs ( job_id, job_title, min_salary, max_salary )
  VALUES ( p_job_id, p_job_title, p_min_salary, p_max_salary );
  END add_job;

  PROCEDURE update_department
    ( p_department_id IN departments.department_id%TYPE,
      p_department_name IN departments.department_name%TYPE := NULL,
      p_manager_id IN departments.manager_id%TYPE := NULL,
      p_update_manager_id IN BOOLEAN := FALSE )
  IS
  BEGIN
    IF ( p_update_manager_id ) THEN
     UPDATE departments
     SET department_name = NVL( p_department_name, department_name ),
     manager_id = p_manager_id
     WHERE department_id = p_department_id;
    ELSE
     UPDATE departments
     SET department_name = NVL( p_department_name, department_name )
     WHERE department_id = p_department_id;
    END IF;
  END update_department;

  FUNCTION add_department
    ( p_department_name IN departments.department_name%TYPE,
      p_manager_id IN departments.manager_id%TYPE )
      RETURN departments.department_id%TYPE
  IS
   l_department_id departments.department_id%TYPE := departments_sequence.NEXTVAL;
  BEGIN
   INSERT INTO departments ( department_id, department_name, manager_id )
   VALUES ( l_department_id, p_department_name, p_manager_id );
   RETURN l_department_id;
  END add_department;
END admin_pkg;
/

COMMIT;

-----------------------------------
-- Grant privileges for packages --
-----------------------------------

CONNECT app_code/&pass

GRANT EXECUTE ON employees_pkg TO app_admin_user;
GRANT EXECUTE ON employees_pkg TO app_user;

CONNECT app_admin/&pass

GRANT EXECUTE ON admin_pkg TO app_admin_user;

COMMIT;

----------------------
-- PACKAGE SYNONYMS --
----------------------

CONNECT app_admin_user/&pass

CREATE OR REPLACE SYNONYM employees_pkg
FOR app_code.employees_pkg;

CREATE OR REPLACE SYNONYM admin_pkg
FOR app_admin.admin_pkg;

CONNECT app_user/&pass

CREATE OR REPLACE SYNONYM employees_pkg
FOR app_code.employees_pkg;

COMMIT;










--Wie dit leest: die fucking docent suckt ass, hoe verwacht die dikke lul nu
--dat iedereen zonder fouten 3241 lijnen code schrijft
--
--Die wilt gewoon dat iedereen buist want die krijgt daar persoonlijk genot van
--Dit is de enige verklaring














-- © 2023 Daan van Luijk, Christos Lappas