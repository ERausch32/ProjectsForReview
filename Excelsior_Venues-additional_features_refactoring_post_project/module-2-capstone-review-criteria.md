

# Module 2 Capstone Review Qualifications


2 Levels of Criteria that match “real world” project criteria



*   Product Owner Acceptance
*   Code Review
----

## Product Owner Acceptance

The project will be judged successful or failed by the state of the Minimum Viable Product (MVP).   MVP is defined as ALL requirements not marked as BONUS



*   MVP complete = can release product
*   MVP incomplete = cannot release product
----

### Product Owner Acceptance Criteria



1. Did you complete all MVP requirements?
    - Not complete without Tests
1. Complete BONUS requirements LAST
    - 99% of requirements with 100% of bonuses = failed project
    - 100% of requirements with 0% bonuses = successful project
    - REMEMBER, a requirement without tests is INCOMPLETE, so 
        - 100% of requirements 50% tested with 100% of bonuses = failed projected

----
## Code Review

Good code matters!   So the project will also be judged on the quality of the code.   The code must also “pass” code review.   During a code review, the code is examined for the following

----
### Code Review Criteria



3. Can you explain your code
4. Does it avoid common OOP errors:
    1. Do methods do “one thing”?
    2. Are methods small (not large)
    3. Do methods return to the calling program?
    4. Are most methods testable?
    5. Is all Console input and output isolated to one class?
    6. Do names (variables, classes, and methods) represent the thing they do or contain?
4. Does it make good uses of classes and methods
5. Does it make good use of the language
6. Does it properly use the Database
6. Is data in one place
6. Is data mostly kept in the database, not in collections in the program
6. Is data selected by SELECT statements, not by looping through collections
6. Is functionality in one place
6. Is functionality in the place that makes sense
7. Does it have any errors that would cause it to eventually fail

----
## Final Score
The final score will be one of the following:

----
1. We can’t release this (Fail)
    1. Product Owner does not Approve  
        1. MVP incomplete OR not working as expected
            OR
        2. Failed code review for major reasons
            1.  Parts would require a major rewrite
            2. The application has critical errors that would not allow it to function long-term
-----
2. We can release this, but X needs to be fixed  (Success)
    1. Product owner approved
    2. Failed code review for minor reasons
        1. Variable naming
       2. Incomplete testing
        3. Minor class or method issues
----
3. We can release this as is  (Success)

    1. Product owner approved 
    2. Passes code review with no revisions